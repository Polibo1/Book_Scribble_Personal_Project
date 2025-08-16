using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.IO;
using BOOK_SCRIBBLE_PROJECT.Models;

namespace BOOK_SCRIBBLE_PROJECT.Data
{
    public class DatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager()
        {
            // 실행 파일이 있는 디렉터리 경로를 사용하여 상대 경로 설정
            //string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //string dbPath = Path.Combine(appDirectory, "BOOK_SCRIBBLE_DB.db");
            string dbPath = "C:\\Boeun\\LMS6_BOOK_SCRIBBLE\\BOOK_SCRIBBLE_2025_0816\\BOOK_SCRIBBLE_PROJECT\\BOOK_SCRIBBLE_DB.db";
            _connectionString = $"Data Source={dbPath}";
        }

        // DB의 BOOKS 테이블에서 모든 책 정보를 가져오는 메서드
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
    }
}