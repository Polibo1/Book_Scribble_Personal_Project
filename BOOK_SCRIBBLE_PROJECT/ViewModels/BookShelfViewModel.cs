using System.Collections.ObjectModel;
using System.Linq;
using BOOK_SCRIBBLE_PROJECT.Models;
using BOOK_SCRIBBLE_PROJECT.Data;

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

            //if (books == null || !books.Any())
            //{
            //    // DB에 책이 없을 경우 임시 데이터를 생성
            //    books = new List<BOOK_SCRIBBLE_PROJECT.Models.Book>
            //    {
            //        new BOOK_SCRIBBLE_PROJECT.Models.Book
            //        {
            //            TITLE = "임시 책 1",
            //            AUTHOR = "임시 저자 1",
            //            COVER_PATH = "C:\\Boeun\\LMS6_BOOK_SCRIBBLE\\BOOK_SCRIBBLE_2025_0816\\COVER\\dream.jpg" // 실제 이미지 경로로 수정하세요
            //        },
            //        new BOOK_SCRIBBLE_PROJECT.Models.Book
            //        {
            //            TITLE = "임시 책 2",
            //            AUTHOR = "임시 저자 2",
            //            COVER_PATH = "C:\\Boeun\\LMS6_BOOK_SCRIBBLE\\BOOK_SCRIBBLE_2025_0816\\COVER\\thousand_year.jpg"
            //        },
            //        new BOOK_SCRIBBLE_PROJECT.Models.Book
            //        {
            //            TITLE = "임시 책 3",
            //            AUTHOR = "임시 저자 3",
            //            COVER_PATH = "C:\\Boeun\\LMS6_BOOK_SCRIBBLE\\BOOK_SCRIBBLE_2025_0816\\COVER\\aurora.jpg"
            //        }
            //    };
            //}




            if (books != null && books.Any())
            {
                var bookViewModels = books.Select(b => new BookViewModel
                {
                    Title = b.TITLE,
                    Author = b.AUTHOR,
                    CoverImage = b.COVER_PATH // DB에서 가져온 경로를 사용합니다.
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