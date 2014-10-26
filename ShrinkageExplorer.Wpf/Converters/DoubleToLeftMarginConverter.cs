using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ShrinkageExplorer.Wpf.Converters
{
  public class DoubleToLeftMarginConverter : MarkupExtension, IValueConverter
  {
    private DoubleToLeftMarginConverter _converter;
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return _converter ?? (_converter = new DoubleToLeftMarginConverter());
    }

    #region Члены IValueConverter

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return new Thickness((double)value, 0, 0, 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
