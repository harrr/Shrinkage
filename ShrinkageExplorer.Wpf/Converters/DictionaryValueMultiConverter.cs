using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Media3D;

namespace ShrinkageExplorer.Wpf.Converters
{
  class DictionaryValueMultiConverter : MarkupExtension, IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      var dictionary = values[0] as Dictionary<int, IEnumerable<Point3D>>;
      var key = System.Convert.ToInt32(values[1]);
      if (dictionary == null || dictionary.Count == 0 || key == 0)
        return null;
      return dictionary[key];
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    private DictionaryValueMultiConverter _converter;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return _converter ?? (_converter = new DictionaryValueMultiConverter());
    }
  }
}
