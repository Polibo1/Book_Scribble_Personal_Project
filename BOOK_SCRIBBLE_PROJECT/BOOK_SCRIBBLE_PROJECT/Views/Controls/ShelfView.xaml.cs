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

using System.Windows.Controls;


// ShelfView는 DataContext = Shelf 를 기대합니다(ItemsControl에서 자동으로 그렇게 바인딩할 거예요).
namespace BOOK_SCRIBBLE_PROJECT.UserControls
{
    public partial class ShelfView : UserControl
    {
        public ShelfView() => InitializeComponent();
    }
}