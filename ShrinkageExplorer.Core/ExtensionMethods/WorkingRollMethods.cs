using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.ExtensionMethods
{
  public static class WorkingRollMethods
  {
    public static float GetVelocityRatioWith(this IWorkingRoll thisRoll, IWorkingRoll anotherRoll)
    {
      return thisRoll.Velocity / anotherRoll.Velocity;
    }
  }
}
