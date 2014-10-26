using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShrinkageExplorer.Core.DataClasses;
using ShrinkageExplorer.Core.Models;

namespace ShrinkageExplorer.Data
{
  static class DbClassesToCoreConverters
  {


    static public MaterialProperty ToCoreProperty(this Parameter dbParameter)
    {
      return new MaterialProperty
      {
        Name = dbParameter.Name,
        ShortName = dbParameter.ShortName,
        Unit = dbParameter.Unit,
        Value = "0"
      };
    }

    static public Core.DataClasses.IMaterial ToCoreMaterial(this IMaterial dbMaterial)
    {
      return new Core.DataClasses.IMaterial
      {
        Name = dbMaterial.Name,
        Description = dbMaterial.Description,
        Properties = dbMaterial.MaterialParameters.Select(parameter =>
          new MaterialProperty
          {
            Name = parameter.Parameter.Name,
            ShortName = parameter.Parameter.ShortName,
            Value = parameter.Value,
            Unit = parameter.Parameter.Unit
          })
      };
    }
  }
}
