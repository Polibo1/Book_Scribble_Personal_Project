using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BOOK_SCRIBBLE_PROJECT.UserControls
{
    /// <summary>
    /// MainLayoutView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainLayoutView : UserControl
    {
        public MainLayoutView()
        {
            InitializeComponent();
            this.Loaded += UserControl_Loaded;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 기본: 책장 배경 뷰를 스크롤 자리(ScrollContentHost)에 로드
                ScrollContentHost.Content = new BookShelfBackgroundView();
                if (BtnList != null) BtnList.IsChecked = true;
                if (BtnTower != null) BtnTower.IsChecked = false;
            }
            catch
            {
                // 디자이너 로딩 등으로 이름이 아직 연결되지 않은 상황 대비
            }
        }

        private void BtnList_Checked(object sender, RoutedEventArgs e)
        {
            if (BtnTower != null) BtnTower.IsChecked = false;
            ScrollContentHost.Content = new BookShelfBackgroundView();
        }

        private void BtnTower_Checked(object sender, RoutedEventArgs e)
        {
            if (BtnList != null) BtnList.IsChecked = false;

            // 임시 플레이스홀더 (추후 BookTowerView로 교체)
            var placeholder = new Border
            {
                Padding = new Thickness(24),
                Background = Brushes.Transparent,
                Child = new TextBlock
                {
                    Text = "도서타워 뷰 (추가 예정)",
                    FontSize = 24,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };
            ScrollContentHost.Content = placeholder;
        }
    }
}
