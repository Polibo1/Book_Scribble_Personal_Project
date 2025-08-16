using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BOOK_SCRIBBLE_PROJECT.Converters
{
    public class ImagePathToBitmapConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value as string;

            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            try
            {
                // 경로에 file:/// 접두사를 추가하여 URI를 생성합니다.
                // UriKind.Absolute를 사용하여 절대 경로임을 명시합니다.
                var uri = new Uri(new Uri("file:///"), path);

                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.CreateOptions = BitmapCreateOptions.IgnoreImageCache; // 캐시 무시
                bi.UriSource = uri;
                bi.EndInit();
                bi.Freeze();

                return bi;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}