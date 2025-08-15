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
    /// <summary>
    /// PageLoading.xaml에 대한 상호 작용 논리
    /// </summary>
    //public partial class PageLoading : Page
    //{
    //    //_cts : 취소 토큰 생성기, 기다렸던 작업을 취소할 때 사용
    //    private readonly CancellationTokenSource _cts = new(); // CancellationTokenSource는 비동기 작업을 취소할 때 사용

    //    public PageLoading()
    //    {
    //        InitializeComponent(); // XAML에 적어둔 UI를 실제 C# 객체로 만들어주는 호출
    //    }

    //    // 페이지가 로드될 때 호출되는 이벤트 핸들러
    //    private async void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
    //    {
    //        try
    //        {
    //            await Task.Delay(2000, _cts.Token); // 2초 동안 대기합니다. 2초동안 로딩 페이지 보여줌

    //            // 다음 페이지(MainCalendarPage)로 이동
    //            // NavigationService는 Frame 안의 Page에서 사용할 수 있음
    //            //NavigationService?.Navigate(new PageCalender()); // 주석 처리된 이유: MainCalendarPage가 정의되지 않았거나, 다른 페이지로 이동할 수 있습니다.
    //            //NavigationService?.Navigate(new MainL());
    //        }
    //        catch (TaskCanceledException)
    //        {
    //            // 페이지가 닫히거나 다른 곳으로 이동하면 타이머를 취소합니다.
    //        }
    //    }

    //    // 페이지가 언로드될 때 호출되는 이벤트 핸들러
    //    private void Page_Unloaded(object sender, System.Windows.RoutedEventArgs e)
    //    {
    //        // 페이지가 메모리에서 내려갈 때 타이머를 취소하여 리소스를 정리합니다.
    //        _cts.Cancel();
    //    }
    //}



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
