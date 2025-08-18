using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

namespace BOOK_SCRIBBLE_PROJECT.Models
{
    public partial class Review : ObservableObject
    {
        // REVIEWS 테이블의 기본 키
        [ObservableProperty]
        private int _reviewID;

        // BOOKS 테이블과 연결하는 외래 키
        [ObservableProperty]
        private int _bookID;

        // 독후감의 내용
        [ObservableProperty]
        private string _content;

        // 독후감 작성일
        [ObservableProperty]
        private string _date;
    }
}