using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ShrinkageExplorer.Wpf.Converters
{
  public class BoolToVisibilityConverter : MarkupExtension, IValueConverter
  {
    private BoolToVisibilityConverter _converter;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return _converter ?? (_converter = new BoolToVisibilityConverter());
    }


    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (bool)value
        ? Visibility.Visible
        : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
