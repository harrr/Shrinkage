using System;
using System.Collections.Generic;
using System.Linq;

namespace ShrinkageExplorer.Core.DataClasses
{
  public class Material : IEquatable<Material>
  {
    public Material()
    {
      Properties = new List<MaterialProperty>();
    }

    public Material(string name, string description, IEnumerable<MaterialProperty> properties = null)
    {
      Name = name;
      Description = description;
      Properties = properties == null
        ? new List<MaterialProperty>()
        : new List<MaterialProperty>(properties);
    }
    public Material(IEnumerable<MaterialProperty> properties)
    {
      Properties = new List<MaterialProperty>(properties);
    }

    public string Name { get; set; }

    public string Description { get; set; }

    public IEnumerable<MaterialProperty> Properties { get; set; }

    public double this[string parameterShortName]
    {
      get
      {
        return GetScalarValue(parameterShortName);
      }
    }

    public double this[string parameterShortName, int index]
    {
      get
      {
        return (GetVectorValue(parameterShortName)[index]);
      }
    }

    /// <summary>
    /// Gets a material parameter value or throws a MaterialException
    /// </summary>
    /// <param name="parameterShortName">Short name of material parameter. For example, t0, n, etc</param>
    /// <returns>The parameter's value</returns>
    public double GetScalarValue(string parameterShortName)
    {
      var parameterShortNames = Properties.Select(x => x.ShortName).ToList();
      if (parameterShortNames.Contains(parameterShortName))
        return Properties
                    .Where(x => String.Equals(x.ShortName, parameterShortName))
                    .Select(x => x.NumValue)
                    .First();
      throw new ShrinkageExplorerException("Incorrect parameter name \"" + parameterShortName + "\"");
    }

    public double[] GetVectorValue(string parameterShortName)
    {
      var parameterShortNames = Properties.Select(x => x.ShortName).ToList();
      if (parameterShortNames.Contains(parameterShortName))
        return Properties
                    .Where(x => String.Equals(x.ShortName, parameterShortName))
                    .Select(x => x.ArrValue)
                    .First();
      throw new ShrinkageExplorerException("Incorrect parameter name \"" + parameterShortName + "\"");
    }


    public bool Equals(Material other)
    {
      return Name.Equals(other.Name);
    }
  }
}