using BOOK_SCRIBBLE_PROJECT.Data;
using BOOK_SCRIBBLE_PROJECT.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BOOK_SCRIBBLE_PROJECT.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DatabaseManager _databaseManager;

        [ObservableProperty]
        private object? _currentViewModel;

        // 토글 버튼의 선택 상태를 관리할 속성
        [ObservableProperty]
        private bool _isBookShelfSelected = true;

        [ObservableProperty]
        private bool _isBookTowerSelected = false;
        public MainViewModel()
        {
            // DatabaseManager 인스턴스를 생성하여 데이터베이스 연결을 초기화
            _databaseManager = new DatabaseManager();
            // 앱 시작 시 BookShelfViewModel이 기본으로 보이도록 설정
            CurrentViewModel = new BookShelfViewModel();
        }

        // [RelayCommand]가 이 메서드를 'NavigateToBookShelfCommand'로 만들어줍니다.
        [RelayCommand]
        private void NavigateToBookShelf()
        {
            CurrentViewModel = new BookShelfViewModel();
            Debug.WriteLine("Navigated to BookShelfViewModel.");
            // 도서목록 버튼이 선택되었으므로 다른 버튼은 해제
            IsBookShelfSelected = true;
            IsBookTowerSelected = false;
        }

        // [RelayCommand]가 이 메서드를 'NavigateToBookTowerCommand'로 만들어줍니다.
        [RelayCommand]
        private void NavigateToBookTower()
        {
            // TODO: BookTowerViewModel 인스턴스를 생성하여 할당
            Debug.WriteLine("Navigated to BookTowerViewModel.");
            CurrentViewModel = null; // 임시로 null 설정
            // 도서타워 버튼이 선택되었으므로 다른 버튼은 해제
            IsBookShelfSelected = false;
            IsBookTowerSelected = true;
        }

        [RelayCommand]
        public void NavigateToBookDetail(Book book)
        {
            Debug.WriteLine("클릭한 BOOK_ID : {0}", book.BOOK_ID);
            if (book == null)
            {
                Debug.WriteLine("NavigateToBookDetail: book is null");
                return;
            }
            if (_databaseManager == null)
            {
                Debug.WriteLine("NavigateToBookDetail: _databaseManager is null (should not happen)");
                return;
            }

            // DatabaseManager를 사용하여 독후감과 구절을 가져오기
            var reviews = _databaseManager.GetReviewsByBookId(book.BOOK_ID);
            var quotes = _databaseManager.GetQuotesByBookId(book.BOOK_ID);

            // 가져온 데이터로 BookDetailViewModel을 초기화
            CurrentViewModel = new BookDetailViewModel(book, reviews, quotes);
            Debug.WriteLine("Navigated to BookDetailViewModel.");
        }

        [RelayCommand]
        public void OpenCreateBookView()
        {
            CurrentViewModel = new CreateBookViewModel();
            Console.WriteLine("Navigated to CreateBookViewModel.");
        }
    }
}