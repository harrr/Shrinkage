using System;
using System.Collections.Generic;

namespace ShrinkageExplorer.Core.Interfaces
{
  public interface IMaterial : IEquatable<IMaterial>
  {
    string Name { get; set; }

    string Description { get; set; }

    ICollection<IMaterialProperty> Properties { get; }
  }
}