using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ShrinkageExplorer.Wpf.Converters
{
  class FloatMutiplyBy2Converter : MarkupExtension, IValueConverter
  {
    private FloatMutiplyBy2Converter _by2Converter;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return _by2Converter ?? (_by2Converter = new FloatMutiplyBy2Converter());
    }

    #region Члены IValueConverter

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (float)value * 2;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (float)value / 2;
    }

    #endregion
  }
}
