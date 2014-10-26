using GalaSoft.MvvmLight;

namespace ShrinkageExplorer.Wpf.Utilities
{
  public interface IDialogService
  {
    bool? ShowDialog(ViewModelBase dataContext);
  }
}
