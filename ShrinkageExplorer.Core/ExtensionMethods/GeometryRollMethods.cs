using System;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.ExtensionMethods
{
  public static class GeometryRollMethods
  {
    public static float GetDistanceBetweenCentersWith(this IGeometryRoll thisRoll, IGeometryRoll anotherRoll)
    {
      return (float)Math.Sqrt(Math.Pow(anotherRoll.X - thisRoll.X, 2) 
        + Math.Pow(anotherRoll.Y - thisRoll.Y, 2));
    }

    public static float GetDistanceBetweenSurfacesWith(this IGeometryRoll thisRoll, IGeometryRoll anotherRoll)
    {
      return thisRoll.GetDistanceBetweenCentersWith(anotherRoll) - thisRoll.Radius - anotherRoll.Radius;
    }

    public static bool IsIntersectWith(this IGeometryRoll thisRoll, IGeometryRoll anotherRoll)
    {
      return thisRoll.GetDistanceBetweenSurfacesWith(anotherRoll) < 0;
    }
  }
}
