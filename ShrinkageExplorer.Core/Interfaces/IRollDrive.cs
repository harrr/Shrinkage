using System;
using System.Collections.Generic;

namespace ShrinkageExplorer.Core.Interfaces
{
  /// <summary>
  ///   Класс привода валка
  /// </summary>
  public interface IRollDrive : IEquatable<IRollDrive>
  {
    int Number { get; set; }

    float Temperature { get; set; }

    float Velocity { get; set; }

    float MinVelocity { get; set; }

    float MaxVelocity { get; set; }

    float MinTemperature { get; set; }

    float MaxTemperature { get; set; }

    IWorkingRoll CreateWorkingRoll(IGeometryRoll roll);

    IWorkingRoll CreateWorkingRoll();

    ICollection<IGeometryRoll> GeometryRolls { get; }

    IList<IWorkingRoll> WorkingRolls { get; }

    IRollDrive Clone();
  }
}