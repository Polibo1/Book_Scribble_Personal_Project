using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls; // WPF의 Page, Frame 등
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation; // NavigationService(페이지 이동 도우미)
using System.Windows.Shapes;

using System.Threading; // CancellationToken(취소 토큰)
using System.Threading.Tasks; // Task.Delay(비동기 지연)


using System;
using System.Windows.Threading;


namespace BOOK_SCRIBBLE_PROJECT.Views
{
    public partial class PageLoading : Page
    {
        private DispatcherTimer timer;

        public PageLoading()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            // 현재 페이지의 부모 윈도우를 MainWindow 타입으로 찾음
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                // MainWindow의 ShowMainLayout 메서드 호출
                mainWindow.ShowMainLayout();
            }
        }
    }

}
