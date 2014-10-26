using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ShrinkageExplorer.Wpf.Converters
{
  public class NumberToSequenceConverter : MarkupExtension, IValueConverter
  {
    private NumberToSequenceConverter _converter;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return _converter ?? (_converter = new NumberToSequenceConverter());
    }

    #region Члены IValueConverter

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var number = (int)value;
      var array = new int[number];
      for (int i = 1; i <= number; ++i)
        array[i - 1] = i;
      return array;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
