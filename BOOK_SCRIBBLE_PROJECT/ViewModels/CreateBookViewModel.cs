// FILE: ViewModels/CreateBookViewModel.cs
using BOOK_SCRIBBLE_PROJECT.Data;
using BOOK_SCRIBBLE_PROJECT.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;                // OpenFileDialog
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;                 // Application.Current

namespace BOOK_SCRIBBLE_PROJECT.ViewModels
{
    public partial class CreateBookViewModel : ObservableObject
    {
        private const string CoversDir = @"C:\Boeun\LMS6_BOOK_SCRIBBLE\BOOK_SCRIBBLE_2025_0816\Covers"; // ← 네가 원하는 경로

        // ===== 기본 입력값 =====
        [ObservableProperty] private string? title;           // 제목
        [ObservableProperty] private string? author;          // 저자
        [ObservableProperty] private int? totalPage;       // 총 페이지
        [ObservableProperty] private DateTime? finishDate; // 완독 날짜
        [ObservableProperty] private string? reviewContent;   // 독후감 본문

        // ===== 구절 입력 버퍼 & 리스트 =====
        // 페이지는 문자열로 받아서 Add 시 파싱(빈칸/문자 입력에도 커맨드가 막히지 않게)
        [ObservableProperty] private string? newQuotePageText;
        [ObservableProperty] private string? newQuoteContent;

        [ObservableProperty] private ObservableCollection<QuoteViewModel> quotes = new();
        [ObservableProperty] private QuoteViewModel? selectedQuote;

        // 선택 변경 시 삭제 버튼 CanExecute 갱신
        partial void OnSelectedQuoteChanged(QuoteViewModel? value)
            => RemoveSelectedQuoteCommand.NotifyCanExecuteChanged();

        // ===== 표지 =====
        [ObservableProperty] private string? coverImagePath;

        [RelayCommand]
        private void BrowseCover()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "이미지 파일|*.png;*.jpg;*.jpeg;*.bmp;*.gif|모든 파일|*.*",
                Title = "책 표지 선택"
            };
            if (dlg.ShowDialog() == true)
            {
                // 문서\BookScribble\Covers 폴더에 복사
                var destDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "BookScribble", "Covers");
                Directory.CreateDirectory(destDir);

                var fileName = Path.GetFileName(dlg.FileName);
                var target = Path.Combine(destDir, fileName);

                // 이름 충돌 시 _1, _2 …
                if (File.Exists(target))
                {
                    var stem = Path.GetFileNameWithoutExtension(fileName);
                    var ext = Path.GetExtension(fileName);
                    int i = 1;
                    while (File.Exists(target = Path.Combine(destDir, $"{stem}_{i}{ext}"))) i++;
                }

                File.Copy(dlg.FileName, target);
                CoverImagePath = target;   // 미리보기 + 나중에 DB 저장
            }
        }

        [RelayCommand]
        private void ClearCover() => CoverImagePath = null;

        // ===== 구절 추가/삭제 =====
        [RelayCommand]
        private void AddQuote()
        {
            var content = (NewQuoteContent ?? "").Trim();
            if (string.IsNullOrEmpty(content)) return;

            int page = 0;
            int.TryParse(NewQuotePageText, out page);

            Quotes.Add(new QuoteViewModel
            {
                PageNum = page,
                Content = content
            });

            // 입력 초기화
            NewQuotePageText = string.Empty;
            NewQuoteContent = string.Empty;
        }

        [RelayCommand(CanExecute = nameof(CanRemoveSelectedQuote))]
        private void RemoveSelectedQuote()
        {
            if (SelectedQuote != null)
                Quotes.Remove(SelectedQuote);
        }

        private bool CanRemoveSelectedQuote() => SelectedQuote != null;

        // ===== 저장/취소 =====
        [RelayCommand]
        private void Save()
        {
            // 예외 처리 및 유효성 검사
            if (string.IsNullOrWhiteSpace(Title))
            {
                MessageBox.Show("제목을 입력해주세요.", "입력 오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(Author))
            {
                MessageBox.Show("저자를 입력해주세요.", "입력 오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (TotalPage <= 0 || TotalPage is null)
            {
                MessageBox.Show("총 페이지 수를 올바르게 입력해주세요.", "입력 오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 표지 이미지가 선택되지 않은 경우 기본 이미지로 설정
            if (string.IsNullOrWhiteSpace(CoverImagePath))
            {
                Console.WriteLine("No cover image selected, using default.");
                CoverImagePath = Path.Combine(CoversDir, "default_cover.png");
            }
            Console.WriteLine($"Cover image path: {CoverImagePath}");

            // 1) DatabaseManager 가져오기 (네 프로젝트 방식에 맞게)
            var dbManager = ((App)Application.Current).DbManager;  // 예시: App에 등록돼 있다면
            Console.WriteLine("Using DatabaseManager instance from App.");

            // 2) 책 저장 → 새로 발급된 BookID 받기
            int newBookId = dbManager.InsertBookAndReturnId(new Book
            {
                TITLE = Title,
                AUTHOR = Author,
                TOTAL_PAGE = TotalPage ?? 0, // Nullable<int> 처리
                COVER_PATH = CoverImagePath,
                FINISH_DATE = FinishDate?.ToString("yyyy-MM-dd"), // DateTime? 형식 처리
                //UPDAATE_DATE = DateTime.Now.ToString("yyyy-MM-dd")
                //FINISH_DATE = FinishDate.HasValue ? FinishDate.Value.ToString("yyyy-MM-dd") : null,
                //FINISH_DATE = "2025-01-01" // 예시로 고정된 날짜, 실제로는 FinishDate?.ToString("yyyy-MM-dd") 사용
            });
            Console.WriteLine($"New book ID: {newBookId}");

            // 리뷰 저장 (있다면)
            if (!string.IsNullOrWhiteSpace(ReviewContent))
            {
                Console.WriteLine($"Saving review for book ID {newBookId}: {ReviewContent.Trim()}");
                dbManager.InsertReview(new Review
                {
                    BookID = newBookId,
                    Content = ReviewContent.Trim(),
                    Date = DateTime.Now.ToString("yyyy-MM-dd")
                });
            }
            else
            {
                Console.WriteLine("No review content provided.");
            }

            // 구절 저장 (있다면)
            if (Quotes.Count > 0)
            {
                Console.WriteLine($"Saving {Quotes.Count} quotes for book ID {newBookId}.");
                foreach (var quote in Quotes)
                {
                    dbManager.InsertQuote(new Quote
                    {
                        BookID = newBookId,
                        Content = quote.Content.Trim(),
                        PageNum = quote.PageNum
                    });
                }
            }
            else
            {
                Console.WriteLine("No quotes provided.");
            }


            // 저장 후 목록 화면으로 복귀
            if (Application.Current?.MainWindow?.DataContext is MainViewModel main)
                main.CurrentViewModel = new BookShelfViewModel();
        }

        [RelayCommand]
        private void Cancel()
        {
            if (Application.Current?.MainWindow?.DataContext is MainViewModel main)
                main.CurrentViewModel = new BookShelfViewModel();
        }
    }

    // 뷰 전용 간단 VM (DB 모델과 분리)
    public class QuoteViewModel
    {
        public int PageNum { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
