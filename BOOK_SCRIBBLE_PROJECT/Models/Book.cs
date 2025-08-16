using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOOK_SCRIBBLE_PROJECT.Models
{
    [Table("BOOKS")]
    public class Book
    {
        [Key]
        [Column("BOOK_ID")]
        public int BOOK_ID { get; set; }

        [Column("TITLE")]
        public string TITLE { get; set; }

        [Column("AUTHOR")]
        public string AUTHOR { get; set; }

        [Column("TOTAL_PAGE")]
        public int TOTAL_PAGE { get; set; }

        [Column("FINISH_DATE")]
        public string FINISH_DATE { get; set; }

        [Column("COVER_PATH")]
        public string COVER_PATH { get; set; }

        [Column("UPDATE_DATE")]
        public string UPDATE_DATE { get; set; }
    }
}