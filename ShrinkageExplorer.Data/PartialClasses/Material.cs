using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Data
{
  public partial class Material : IMaterial
  {
    #region Члены IMaterial

    public ICollection<IMaterialProperty> Properties
    {
      get
      {
        return MaterialParameters;
      }
    }

    #endregion

    #region Члены IEquatable<IMaterial>

    public bool Equals(IMaterial other)
    {
      return Name.Equals(other.Name);
    }

    #endregion
  }
}
