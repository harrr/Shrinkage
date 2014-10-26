using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ShrinkageExplorer.Wpf.Converters
{
  class InverseBoolConverter : MarkupExtension, IValueConverter
  {
    private InverseBoolConverter _converter;
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return _converter ?? (_converter = new InverseBoolConverter());
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return !(bool)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
