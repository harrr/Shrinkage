using System.Windows;

namespace ShrinkageExplorer.Wpf.Views.Common
{
  /// <summary>
  /// Description for RollPropertiesView.
  /// </summary>
  public partial class RollPropertiesView
  {

    public RollPropertiesView()
    {
      InitializeComponent();
    }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = true;
    }
  }
}