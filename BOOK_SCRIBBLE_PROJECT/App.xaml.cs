using BOOK_SCRIBBLE_PROJECT.Data;   // DatabaseManager 네임스페이스에 맞게
using Microsoft.Data.Sqlite; // 네가 쓰는 Sqlite 네임스페이스 유지
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows;

namespace BOOK_SCRIBBLE_PROJECT
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // ✅ 전역에서 꺼내 쓰는 싱글톤
        public DatabaseManager DbManager { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // DB 경로 만들기 (문서\BookScribble\books.db 예시)
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                    "BookScribble");
            Directory.CreateDirectory(dir);
            var dbPath = Path.Combine(dir, "books.db");
            var connStr = $"Data Source={dbPath}";

            // ✅ 전역 인스턴스 생성
            DbManager = new DatabaseManager();

            // (선택) XAML에서 StaticResource로도 접근할 수 있게 리소스에 넣어두기
            Resources["DbManager"] = DbManager;
        }
    }

}
