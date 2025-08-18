using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BOOK_SCRIBBLE_PROJECT.Models
{
    public partial class Quote : ObservableObject
    {
        // QUOTES 테이블의 기본 키
        [ObservableProperty]
        private int _quoteID;

        // BOOKS 테이블과 연결하는 외래 키
        [ObservableProperty]
        private int _bookID;

        // 구절의 내용
        [ObservableProperty]
        private string _content;

        // 구절이 있는 페이지 번호
        [ObservableProperty]
        private int _pageNum;
    }
}