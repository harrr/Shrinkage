using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ShrinkageExplorer.Wpf.Converters
{
  class CalculationErrorMultiConverter : MarkupExtension, IMultiValueConverter
  {
    private CalculationErrorMultiConverter _converter;
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return _converter ?? (_converter = new CalculationErrorMultiConverter());
    }

    #region Члены IMultiValueConverter

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      var result = (double)values[0];
      var error = (float)values[1];
      return String.Format("{0:P}±{1:P}", result, Math.Abs(result * error));
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
