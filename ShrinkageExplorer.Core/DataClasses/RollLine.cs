using System;
using System.Collections.Generic;
using System.Linq;

namespace ShrinkageExplorer.Core.DataClasses
{
  public class RollLine : IEquatable<RollLine>
  {
    public RollLine()
    {
    }

    public RollLine(string name, float airTemperature, float rollCoefficient, float airCoefficient)
      : this()
    {
      Name = name;
      AirTemperature = airTemperature;
      RollCoefficient = rollCoefficient;
      AirCoefficient = airCoefficient;
    }

    public RollLine(IEnumerable<RollDrive> drives)
    {
      Drives = new List<RollDrive>(drives);
    }

    public RollLine(string name, float airTemperature, float rollCoefficient, float airCoefficient,
      IEnumerable<RollDrive> drives)
    {
      Name = name;
      AirTemperature = airTemperature;
      RollCoefficient = rollCoefficient;
      AirCoefficient = airCoefficient;
      Drives = new List<RollDrive>(drives);
    }


    public RollLine(RollLine line)
      : this(line.Name, line.AirTemperature, line.RollCoefficient, line.AirCoefficient)
    {
      Drives = line.Drives.Select(drive => new RollDrive(drive)).ToList();
    }


    public string Name { get; set; }

    public IList<RollDrive> Drives { get; set; }

    public float AirTemperature { get; set; }

    public float RollCoefficient { get; set; }

    public float AirCoefficient { get; set; }

    public IEnumerable<WorkingRoll> WorkingRolls
    {
      get
      {
        return Drives
          .SelectMany(drive => drive.WorkingRolls);
      }
    }

    /*public void AddRoll(GeometryRoll roll, RollDrive drive)
    {
      if (!Drives.Contains(drive))
        throw new ShrinkageExplorerException("The drive is not belonging to this line");
      drive.AddRoll(roll);
    }*/

    public bool Equals(RollLine other)
    {
      if (this != other)
        return false;
      if (!String.Equals(Name, other.Name))
        return false;
      if (Drives.Count() != other.Drives.Count())
        return false;
      return !Drives.Where((t, i) => !t.Equals(other.Drives.ElementAt(i))).Any();
    }
  }
}