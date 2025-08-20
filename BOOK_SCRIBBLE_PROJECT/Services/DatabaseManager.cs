using BOOK_SCRIBBLE_PROJECT.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BOOK_SCRIBBLE_PROJECT.Data
{
    public class DatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager()
        {

            // 1) 원하는 경로 지정
            string dbPath = "C:\\Boeun\\LMS6_BOOK_SCRIBBLE\\BOOK_SCRIBBLE_2025_0816\\BOOK_SCRIBBLE_PROJECT\\BOOK_SCRIBBLE_DB.db";

            // 2) 폴더 보장
            var dir = Path.GetDirectoryName(dbPath)!;
            Directory.CreateDirectory(dir);

            // 3) 폴더 쓰기 권한 간단 체크(선택)
            try
            {
                var test = Path.Combine(dir, ".writetest.tmp");
                File.WriteAllText(test, "ok");
                File.Delete(test);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"DB 폴더에 쓰기 권한이 없습니다: {dir}", ex);
            }

            // 4) 기존 파일이 ReadOnly면 해제
            if (File.Exists(dbPath))
            {
                var attr = File.GetAttributes(dbPath);
                if (attr.HasFlag(FileAttributes.ReadOnly))
                    File.SetAttributes(dbPath, attr & ~FileAttributes.ReadOnly);
            }

            // 5) 쓰기 가능 + 없으면 생성 모드로 연결 문자열 구성
            var csb = new SqliteConnectionStringBuilder
            {
                DataSource = dbPath,
                Mode = SqliteOpenMode.ReadWriteCreate, // ★ 중요
                Cache = SqliteCacheMode.Shared
            };
            _connectionString = csb.ToString();

        }

        public List<Book> GetAllBooks()
        {
            var books = new List<Book>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT BOOK_ID, TITLE, AUTHOR, TOTAL_PAGE, FINISH_DATE, COVER_PATH, UPDATE_DATE FROM BOOKS";
                    using (var cmd = new SqliteCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(new Book
                            {
                                BOOK_ID = reader.GetInt32(0),
                                TITLE = reader.GetString(1),
                                AUTHOR = reader.GetString(2),
                                TOTAL_PAGE = reader.GetInt32(3),
                                FINISH_DATE = reader.IsDBNull(4) ? null : reader.GetString(4),
                                COVER_PATH = reader.IsDBNull(5) ? null : reader.GetString(5),
                                UPDATE_DATE = reader.IsDBNull(6) ? null : reader.GetString(6),
                            });
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    System.Windows.MessageBox.Show($"데이터베이스 연결 오류: {ex.Message}", "오류", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return new List<Book>(); // ← null 대신 빈 리스트
                }
            }
            return books;
        }

        public ObservableCollection<Review> GetReviewsByBookId(int bookId)
        {
            var reviews = new ObservableCollection<Review>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT REVIEW_ID, BOOK_ID, CONTENT, DATE FROM REVIEWS WHERE BOOK_ID = @bookId";
                Debug.WriteLine("sql: " + sql);
                Debug.WriteLine("bookId: " + bookId);
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@bookId", bookId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reviews.Add(new Review
                            {
                                ReviewID = reader.GetInt32(0),
                                BookID = reader.GetInt32(1),
                                Content = reader.GetString(2),
                                Date = reader.IsDBNull(3) ? null : reader.GetString(3)
                            });
                        }
                    }
                }
            }
            return reviews;
        }

        public ObservableCollection<Quote> GetQuotesByBookId(int bookId)
        {
            var quotes = new ObservableCollection<Quote>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT QUOTE_ID, BOOK_ID, CONTENT, PAGE_NUM FROM QUOTES WHERE BOOK_ID = @bookId";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@bookId", bookId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            quotes.Add(new Quote
                            {
                                QuoteID = reader.GetInt32(0),
                                BookID = reader.GetInt32(1),
                                Content = reader.GetString(2),
                                PageNum = reader.GetInt32(3)
                            });
                        }
                    }
                }
            }
            return quotes;
        }

        public int InsertBookAndReturnId(Book book)
        {
            try
            {
                using var conn = new SqliteConnection(_connectionString);
                conn.Open();
                Console.WriteLine("Database connection established.");

                Console.WriteLine("BeginTransaction()...");
                try
                {
                    using var tx = conn.BeginTransaction();
                    Console.WriteLine("Transaction started.");
                    using var cmd = conn.CreateCommand();
                    Console.WriteLine("Creating command...");
                    cmd.Transaction = tx;
                    Console.WriteLine("Transaction started and command created.");

                    // ✅ UPDATE_DATE까지 함께 저장
                    cmd.CommandText = @"INSERT INTO BOOKS (TITLE, AUTHOR, TOTAL_PAGE, COVER_PATH, FINISH_DATE, UPDATE_DATE)
                                    VALUES (@title, @author, @total, @cover, @finish, @ud);";

                    cmd.Parameters.AddWithValue("@title", (object?)book.TITLE ?? string.Empty);
                    cmd.Parameters.AddWithValue("@author", (object?)book.AUTHOR ?? string.Empty);
                    cmd.Parameters.AddWithValue("@total", (object?)book.TOTAL_PAGE ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@cover", (object?)book.COVER_PATH ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@finish", (object?)book.FINISH_DATE ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ud", DateTime.Now.ToString("yyyy-MM-dd"));
                    Console.WriteLine("Executing INSERT command...");
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("INSERT command executed successfully.");

                    // 같은 연결/트랜잭션에서 방금 INSERT한 PK 얻기
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT last_insert_rowid();";
                    var obj = cmd.ExecuteScalar();

                    if (obj == null || obj == DBNull.Value)
                        throw new InvalidOperationException("last_insert_rowid()가 값을 반환하지 않았습니다.");

                    tx.Commit();
                    return Convert.ToInt32(Convert.ToInt64(obj));
                }
                catch (SqliteException ex)
                {
                    MessageBox.Show($"BeginTransaction 실패: {ex.Message}", "DB 오류");
                    throw;
                }

            }
            catch (SqliteException ex)
            {
                MessageBox.Show($"책 저장 중 SQLite 오류: {ex.SqliteErrorCode} - {ex.Message}",
                                "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"책 저장 중 오류: {ex.Message}",
                                "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }




        public void InsertReview(Review review)
        {
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    conn.Open();
                    const string sql = @"INSERT INTO REVIEWS (BOOK_ID, CONTENT, DATE)
                                     VALUES (@bookId, @content, @date)";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@bookId", review.BookID);
                        cmd.Parameters.AddWithValue("@content", review.Content ?? string.Empty);
                        cmd.Parameters.AddWithValue("@date", review.Date ?? DateTime.Now.ToString("yyyy-MM-dd"));
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"리뷰 저장 중 오류: {ex.Message}",
                    "DB 오류", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                throw;
            }
        }

        public void InsertQuote(Quote quote)
        {
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    conn.Open();

                    if (quote == null)
                    {
                        Debug.WriteLine("AddQuote: quote is null");
                        return;
                    }

                    string sql = "INSERT INTO QUOTES (BOOK_ID, CONTENT, PAGE_NUM) VALUES (@bookId, @content, @pageNum)";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@bookId", quote.BookID);
                        cmd.Parameters.AddWithValue("@content", quote.Content);
                        cmd.Parameters.AddWithValue("@pageNum", quote.PageNum);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"구절 저장 중 오류: {ex.Message}",
                    "DB 오류", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                throw;
            }

        }

    }
}