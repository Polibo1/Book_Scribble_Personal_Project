using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.IO;
using BOOK_SCRIBBLE_PROJECT.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BOOK_SCRIBBLE_PROJECT.Data
{
    public class DatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager()
        {
            string dbPath = "C:\\Boeun\\LMS6_BOOK_SCRIBBLE\\BOOK_SCRIBBLE_2025_0816\\BOOK_SCRIBBLE_PROJECT\\BOOK_SCRIBBLE_DB.db";
            _connectionString = $"Data Source={dbPath}";
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
                    return null;
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
    }
}