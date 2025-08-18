using System.Collections.ObjectModel;
using System.Linq;
using BOOK_SCRIBBLE_PROJECT.Models;
using BOOK_SCRIBBLE_PROJECT.Data;
using BOOK_SCRIBBLE_PROJECT.ViewModels; // MainViewModel을 참조하기 위해 추가

// 이 뷰모델은 책장을 구성하는 역할
namespace BOOK_SCRIBBLE_PROJECT.ViewModels
{
    public class BookShelfViewModel
    {
        public ObservableCollection<ShelfViewModel> Shelves { get; set; }

        public BookShelfViewModel()
        {
            Shelves = new ObservableCollection<ShelfViewModel>();
            LoadBooksFromDatabase();
        }

        private void LoadBooksFromDatabase()
        {
            var dbManager = new DatabaseManager();
            var books = dbManager.GetAllBooks(); // DB에서 책 목록을 가져옵니다.


            if (books != null && books.Any())
            {
                var bookViewModels = books.Select(b => new BookViewModel
                {
                    Title = b.TITLE, // DB에서 가져온 제목 사용
                    Author = b.AUTHOR, // DB에서 가져온 저자 사용
                    CoverImage = b.COVER_PATH, // DB에서 가져온 경로 사용
                    // 이렇게 해야 나중에 클릭했을 때, 커맨드 파라미터로 * *정확한 Book(모델) * *을 넘길 수 있어.
                    // MainViewModel.NavigateToBookDetail(Book book)은 Book을 받는 시그니처이니까 이게 핵심!
                    Original = b // 클릭 시 상세로 보낼 원본 모델
                }).ToList();

                int booksPerShelf = 3;
                int bookCount = bookViewModels.Count;
                int shelfCount = (bookCount + booksPerShelf - 1) / booksPerShelf;

                for (int i = 0; i < shelfCount; i++)
                {
                    var shelfViewModel = new ShelfViewModel();
                    var booksForCurrentShelf = bookViewModels.Skip(i * booksPerShelf).Take(booksPerShelf).ToList();

                    foreach (var book in booksForCurrentShelf)
                    {
                        shelfViewModel.Books.Add(book);
                    }
                    Shelves.Add(shelfViewModel);
                }
            }
        }
    }
}