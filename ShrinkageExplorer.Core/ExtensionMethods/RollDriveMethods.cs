using System.Linq;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.ExtensionMethods
{
  public static class RollDriveMethods
  {
    public static void AddRoll(this IRollDrive thisDrive, IGeometryRoll addedRoll)
    {
      thisDrive.WorkingRolls.Add(thisDrive.CreateWorkingRoll(addedRoll));
    }

    public static void RemoveRoll(this IRollDrive thisDrive, IGeometryRoll removedRoll)
    {
      var workingRolls = thisDrive.WorkingRolls;
      workingRolls.Remove(workingRolls.First(r => r.Equals(removedRoll)));
    }
  }
}
