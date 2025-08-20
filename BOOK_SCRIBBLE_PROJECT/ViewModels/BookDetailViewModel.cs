using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using BOOK_SCRIBBLE_PROJECT.Models;
using System.Diagnostics;

namespace BOOK_SCRIBBLE_PROJECT.ViewModels
{
    public partial class BookDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _author;

        [ObservableProperty]
        private int _totalPage;

        [ObservableProperty]
        private string _finishDate;

        [ObservableProperty]
        private ObservableCollection<Review> _reviews;

        [ObservableProperty]
        private ObservableCollection<Quote> _quotes;

        [ObservableProperty]
        private string _reviewContent = "작성된 독후감이 없습니다.";

        [ObservableProperty]
        private string _quoteContent = "마음에 드는 구절이 없습니다.";

        public BookDetailViewModel(Book book, ObservableCollection<Review> reviews, ObservableCollection<Quote> quotes)
        {
            Title = book.TITLE;
            Author = book.AUTHOR;
            TotalPage = book.TOTAL_PAGE;
            FinishDate = book.FINISH_DATE;
            Debug.WriteLine($"BookDetailViewModel initialized with Title: {Title}, Author: {Author}, TotalPage: {TotalPage}, FinishDate: {FinishDate}");

            // reviews 컬렉션이 비어있는 경우, 기본 메시지 표시
            if (reviews == null || reviews.Count == 0)
            {
                Reviews = new ObservableCollection<Review>();
                ReviewContent = "작성된 독후감이 없습니다.";
            }
            else
            {
                Reviews = reviews;
                Console.WriteLine($"리뷰 내용: {reviews.First().Content}");
                ReviewContent = reviews.First().Content; // 첫 번째 독후감 내용을 표시 (여러 개일 경우)
            }

            // quotes 컬렉션이 비어있는 경우, 기본 메시지 표시
            if (quotes == null || quotes.Count == 0)
            {
                Quotes = new ObservableCollection<Quote>();
                QuoteContent = "마음에 드는 구절이 없습니다.";
            }
            else
            {
                Quotes = quotes;
                QuoteContent = quotes.First().Content; // 첫 번째 구절 내용을 표시
            }
        }
    }
}