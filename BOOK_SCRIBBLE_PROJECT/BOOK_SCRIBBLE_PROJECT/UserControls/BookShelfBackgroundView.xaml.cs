using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Collections.ObjectModel;
using System.ComponentModel;
using BOOK_SCRIBBLE_PROJECT.Data;
using BOOK_SCRIBBLE_PROJECT.Models;

namespace BOOK_SCRIBBLE_PROJECT.ViewModels
{
    public class BookShelfBackgroundViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseManager _db;
        public ObservableCollection<Shelf> Shelves { get; } = new();

        public BookShelfBackgroundViewModel(DatabaseManager db)
        {
            _db = db;
            LoadShelves();
        }

        private void LoadShelves()
        {
            var books = _db.GetAllBooks();  // List<Book>

            Shelves.Clear();
            for (int i = 0; i < books.Count; i += 3)
            {
                var shelf = new Shelf();
                for (int j = 0; j < 3 && i + j < books.Count; j++)
                    shelf.Books.Add(books[i + j]);

                // 3칸 맞추기(빈 슬롯)
                while (shelf.Books.Count < 3)
                    shelf.Books.Add(new Book()); // COVER_PATH=null → 플레이스홀더 노출

                Shelves.Add(shelf);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
