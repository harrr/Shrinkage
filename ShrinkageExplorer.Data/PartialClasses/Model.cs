using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Core.Models;

namespace ShrinkageExplorer.Data
{
  public partial class Model : IModel
  {
    private static readonly Dictionary<string, Type> ExportedTypes;

    static Model()
    {
      ExportedTypes = AppDomain.CurrentDomain
        .GetAssemblies()
        .Where(asm => !asm.IsDynamic)
        .SelectMany(asm => asm.GetExportedTypes())
        .Where(type => type.IsSubclassOf(typeof(ShrinkageModel)))
        .ToDictionary(type => type.Name, type => type);
    }

    public ShrinkageModel MathModel
    {
      get
      {
        if (String.IsNullOrWhiteSpace(ClassName) || !ExportedTypes.ContainsKey(ClassName))
          return null;
        return Activator.CreateInstance(ExportedTypes[ClassName]) as ShrinkageModel;
      }
    }

    public ICollection<IProperty> RequiredProperties
    {
      get { return Parameters; }
    }


    public bool Equals(IModel other)
    {
      return String.Equals(ClassName, other.ClassName);
    }

  }
}
