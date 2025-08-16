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
using System.Threading.Tasks;
using BOOK_SCRIBBLE_PROJECT.Views;
using BOOK_SCRIBBLE_PROJECT.ViewModels;

namespace BOOK_SCRIBBLE_PROJECT
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 2초 동안 대기
            await Task.Delay(2000);

            // 로딩 페이지를 숨김
            LoadingOverlay.Visibility = Visibility.Collapsed;
        }
    }
}