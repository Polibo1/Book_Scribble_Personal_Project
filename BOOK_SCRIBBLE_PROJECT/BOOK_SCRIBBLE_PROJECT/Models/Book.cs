using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK_SCRIBBLE_PROJECT.Models
{
    public class Book
    {
        public int BOOK_ID { get; set; }
        public string TITLE { get; set; }
        public string AUTHOR { get; set; }
        public int TOTAL_PAGE { get; set; }
        public string FINISH_DATE { get; set; }
        public string COVER_PATH { get; set; }
        public string UPDATE_DATE { get; set; }
    }
}