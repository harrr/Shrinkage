using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Data
{
  public partial class Roll : IGeometryRoll
  {
    public bool Equals(IGeometryRoll other)
    {
      return Id == (other as Roll).Id ||
        (X == other.X
             && Y == other.Y
             && Radius == other.Radius
             && Clockwise == other.Clockwise);
    }

    public IGeometryRoll Clone()
    {
      return new Roll
      {
        X = X,
        Y = Y,
        Radius = Radius,
        Clockwise = Clockwise
      };
    }
  }
}
