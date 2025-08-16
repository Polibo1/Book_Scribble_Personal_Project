using BOOK_SCRIBBLE_PROJECT.UserControls;
using BOOK_SCRIBBLE_PROJECT.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BOOK_SCRIBBLE_PROJECT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 앱 시작 시 로딩 페이지를 Frame에 설정
            MainFrame.Content = new PageLoading();
        }

        // PageLoading에서 호출하여 MainLayoutView를 표시하는 메서드
        public void ShowMainLayout()
        {
            MainFrame.Content = new MainLayoutView();
        }
    }
}