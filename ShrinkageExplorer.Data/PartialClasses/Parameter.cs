using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Data
{
  public partial class Parameter : IProperty
  {
    #region Члены IEquatable<IProperty>

    public bool Equals(IProperty other)
    {
      return String.Equals(ShortName, other.ShortName);
    }

    public override int GetHashCode()
    {
      return ShortName.GetHashCode();
    }

    #endregion
  }
}
