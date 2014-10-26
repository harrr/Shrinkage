using ShrinkageExplorer.Wpf.ViewModels.Common;

namespace ShrinkageExplorer.Wpf.Messages
{
  public class AttachNewRollMessage
  {
    public RollDriveViewModel Drive { get; set; }
    public RollLineViewModel Line { get; set; }
  }
}
