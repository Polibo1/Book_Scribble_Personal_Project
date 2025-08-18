using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using BOOK_SCRIBBLE_PROJECT.ViewModels;

namespace BOOK_SCRIBBLE_PROJECT.Converters
{
    public class TypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BookShelfViewModel)
            {
                return "Visible";
            }
            else
            {
                return "Hidden";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}