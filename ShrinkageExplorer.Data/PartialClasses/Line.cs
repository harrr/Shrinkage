using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Data
{
  public partial class Line : IRollLine
  {
    #region Члены IRollLine

    public IEnumerable<IWorkingRoll> WorkingRolls
    {
      get
      {
        return Drives
          .SelectMany(drive => drive.WorkingRolls);
      }
    }

    public IRollLine Clone()
    {
      return new Line
      {
        Name = Name,
        AirCoefficient = AirCoefficient,
        AirTemperature = AirTemperature,
        RollCoefficient = RollCoefficient,
        Drives = Drives.Select(drive => drive.Clone()).ToList()
      };
    }

    public IRollDrive CreateDrive(int number)
    {
      return new Drive { Number = number };
    }

    #endregion

    #region Члены IEquatable<IRollLine>

    public bool Equals(IRollLine other)
    {
      if (this != other)
        return false;
      if (!String.Equals(Name, other.Name))
        return false;
      if (Drives.Count() != other.Drives.Count())
        return false;
      return !Drives.Where((t, i) => !t.Equals(other.Drives.ElementAt(i))).Any();
    }

    #endregion
  }
}
