using System.Windows;

namespace ShrinkageExplorer.Wpf.Views.Common
{
  /// <summary>
  /// Description for DrivePropertiesView.
  /// </summary>
  public partial class DrivePropertiesView 
  {
    /// <summary>
    /// Initializes a new instance of the DrivePropertiesView class.
    /// </summary>
    public DrivePropertiesView()
    {
      InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
    }
  }
}