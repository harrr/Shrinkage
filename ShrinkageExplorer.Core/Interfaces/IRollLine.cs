using System;
using System.Collections.Generic;

namespace ShrinkageExplorer.Core.Interfaces
{
  public interface IRollLine : IEquatable<IRollLine>
  {
    string Name { get; set; }

    IList<IRollDrive> Drives { get; set; }

    float AirTemperature { get; set; }

    float RollCoefficient { get; set; }

    float AirCoefficient { get; set; }

    IEnumerable<IWorkingRoll> WorkingRolls { get; }

    IRollLine Clone();

    IRollDrive CreateDrive(int number);
  }
}