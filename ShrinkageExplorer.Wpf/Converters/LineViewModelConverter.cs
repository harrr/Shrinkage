using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Wpf.ViewModels.Common;

namespace ShrinkageExplorer.Wpf.Converters
{
  public class LineViewModelConverter : MarkupExtension, IValueConverter
  {
    private LineViewModelConverter _converter;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return _converter ?? (_converter = new LineViewModelConverter());
    }

    #region Члены IValueConverter

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value == null ? null : new RollLineViewModel(value as IRollLine);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}