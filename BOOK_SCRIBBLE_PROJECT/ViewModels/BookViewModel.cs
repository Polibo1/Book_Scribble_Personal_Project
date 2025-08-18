using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOK_SCRIBBLE_PROJECT.Models;

namespace BOOK_SCRIBBLE_PROJECT.ViewModels
{
    public class BookViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string CoverImage { get; set; } = string.Empty;

        public Book Original { get; set; }  // 클릭 시 상세로 보낼 원본 모델
    }
}