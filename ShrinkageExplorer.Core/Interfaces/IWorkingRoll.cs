using System;

namespace ShrinkageExplorer.Core.Interfaces
{
  public interface IWorkingRoll : IGeometryRoll, IEquatable<IWorkingRoll>
  {
    float Temperature { get; set; }

    float Velocity { get; set; }

    IRollDrive RollDrive { get; }
  }
}