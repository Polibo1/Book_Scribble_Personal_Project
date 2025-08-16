using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ViewModel에서 Books가 항상 3개가 되도록 padding(빈 Book)까지 넣어줄거임
namespace BOOK_SCRIBBLE_PROJECT.Models
{
    public class Shelf
    {
        public List<Book> Books { get; set; } = new(3);
    }
}
