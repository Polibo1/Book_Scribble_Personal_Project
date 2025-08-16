using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

//namespace BOOK_SCRIBBLE_PROJECT.ViewModels
//{
//    // 클래스 이름이 MainViewModel로 변경됨
//    public partial class MainViewModel : ObservableObject
//    {
//        [ObservableProperty]
//        private object? _currentViewModel;

//        public MainViewModel()
//        {
//            // 앱 시작 시 BookShelfViewModel이 기본으로 보이도록 설정
//            CurrentViewModel = new BookShelfViewModel();
//        }

//        [RelayCommand]
//        private void NavigateToBookShelf()
//        {
//            CurrentViewModel = new BookShelfViewModel();
//        }

//        [RelayCommand]
//        private void NavigateToBookTower()
//        {
//            // TODO: BookTowerViewModel 인스턴스를 생성하여 할당
//        }
//    }
//}


namespace BOOK_SCRIBBLE_PROJECT.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private object? _currentViewModel;

        public MainViewModel()
        {
            // 앱 시작 시 BookShelfViewModel이 기본으로 보이도록 설정
            CurrentViewModel = new BookShelfViewModel();

            // 디버깅을 위한 출력 코드 추가
            if (CurrentViewModel != null)
            {
                Debug.WriteLine($"CurrentViewModel is not null. Type: {CurrentViewModel.GetType().Name}");
            }
            else
            {
                Debug.WriteLine("CurrentViewModel is null.");
            }
        }

        [RelayCommand]
        private void NavigateToBookShelf()
        {
            CurrentViewModel = new BookShelfViewModel();
            Debug.WriteLine("Navigated to BookShelfViewModel.");
        }

        [RelayCommand]
        private void NavigateToBookTower()
        {
            // TODO: BookTowerViewModel 인스턴스를 생성하여 할당
        }
    }
}