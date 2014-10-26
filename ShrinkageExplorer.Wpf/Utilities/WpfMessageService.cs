using System.Windows;

namespace ShrinkageExplorer.Wpf.Utilities
{
  public class WpfMessageService : IMessageService
  {
    public void ShowMessage(string message)
    {
      MessageBox.Show(message);
    }
  }
}
