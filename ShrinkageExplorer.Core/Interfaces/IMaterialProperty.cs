using System;

namespace ShrinkageExplorer.Core.Interfaces
{
  public interface IMaterialProperty : IProperty, IEquatable<IMaterialProperty>
  {
    string Value { get; set; }

    double NumValue { get; }

    double[] ArrValue { get; }
  }
}
