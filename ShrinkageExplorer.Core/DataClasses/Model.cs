using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShrinkageExplorer.Core.Models;

namespace ShrinkageExplorer.Core.DataClasses
{
  public class Model
  {
    public Model()
    {
      _requiredProperties = new List<MaterialProperty>();
    }

    private List<MaterialProperty> _requiredProperties;

    public ShrinkageModel MathModel { get; set; }
    public string ClassName { get; set; }
    public string Name { get; set; }
    public float AverageError { get; set; }

    public IEnumerable<MaterialProperty> RequiredProperties
    {
      get { return _requiredProperties; }
      set { _requiredProperties = new List<MaterialProperty>(value); }
    }

    public void AddProperty(MaterialProperty property)
    {
      if (_requiredProperties.Contains(property))
        throw new ArgumentException("Model already uses this property", "property");
      _requiredProperties.Add(property);
    }

    public void RemoveProperty(MaterialProperty property)
    {
      if (!_requiredProperties.Contains(property))
        throw new ArgumentException("Model does not have this property", "property");
      _requiredProperties.Remove(property);
    }
  }
}
