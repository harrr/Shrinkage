using GalaSoft.MvvmLight;

namespace ShrinkageExplorer.Wpf.Utilities
{
  public class WpfWindowDialogService : IDialogService
  {
    public bool? ShowDialog(ViewModelBase dataContext)
    {
      var window = ViewModelLocator.GetViewForViewModel(dataContext);
      window.Owner.IsEnabled = false;
      var dialogResult = window.ShowDialog();
      window.Owner.IsEnabled = true;
      return dialogResult;
    }

  }
}
