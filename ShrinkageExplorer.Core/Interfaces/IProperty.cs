using System;

namespace ShrinkageExplorer.Core.Interfaces
{
  public interface IProperty : IEquatable<IProperty>
  {
    string Name { get; set; }

    string ShortName { get; set; }

    string Unit { get; set; }
  }
}
