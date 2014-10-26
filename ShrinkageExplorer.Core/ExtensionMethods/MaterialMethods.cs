using System;
using System.Linq;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.ExtensionMethods
{
  public static class MaterialMethods
  {
    public static double GetScalarValue(this IMaterial thisMaterial, string parameterShortName)
    {
      var parameterShortNames = thisMaterial.Properties.Select(x => x.ShortName).ToList();
      if (parameterShortNames.Contains(parameterShortName))
        return thisMaterial.Properties
                    .Where(x => String.Equals(x.ShortName, parameterShortName))
                    .Select(x => x.NumValue)
                    .First();
      throw new ShrinkageExplorerException("Incorrect parameter name \"" + parameterShortName + "\"");
    }

    public static double[] GetVectorValue(this IMaterial thisMaterial, string parameterShortName)
    {
      var parameterShortNames = thisMaterial.Properties.Select(x => x.ShortName).ToList();
      if (parameterShortNames.Contains(parameterShortName))
        return thisMaterial.Properties
                    .Where(x => String.Equals(x.ShortName, parameterShortName))
                    .Select(x => x.ArrValue)
                    .First();
      throw new ShrinkageExplorerException("Incorrect parameter name \"" + parameterShortName + "\"");
    }
  }
}
