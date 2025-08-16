using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

namespace BOOK_SCRIBBLE_PROJECT.ViewModels
{
    public class ShelfViewModel
    {
        public ObservableCollection<BookViewModel> Books { get; set; }

        public ShelfViewModel()
        {
            Books = new ObservableCollection<BookViewModel>();
        }
    }
}