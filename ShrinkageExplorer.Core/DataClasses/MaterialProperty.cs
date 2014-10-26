using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShrinkageExplorer.Core.DataClasses
{
  public class MaterialProperty : IEquatable<MaterialProperty>
  {
    private double[] _arrValue;
    private double? _numValue;


    public string Name { get; set; }

    public string ShortName { get; set; }

    public string Unit { get; set; }

    public string Value { get; set; }


    public double NumValue
    {
      get
      {
        if (Value.Contains(';'))
          throw new ShrinkageExplorerException("Parameter is a vector value");
        return double.Parse(Value);
      }
    }

    public double[] ArrValue
    {
      get
      {
        if (!Value.Contains(';'))
          throw new ShrinkageExplorerException("Parameter is a scalar value");

        var strValues = Value.Split(';');
        _arrValue = new double[strValues.Length];
        for (int i = 0; i < strValues.Length; ++i)
          _arrValue[i] = Double.Parse(strValues[i]);
        return _arrValue;
      }
    }

    public double this[int i]
    {
      get
      {
        if (_arrValue == null)
          throw new ShrinkageExplorerException("Parameter is a scalar value");
        if (i > _arrValue.Length)
          throw new IndexOutOfRangeException("Index is out of range");
        return _arrValue[i];
      }
    }

    public bool Equals(MaterialProperty other)
    {
      return String.Equals(other.ShortName, ShortName);
    }

    public override bool Equals(object obj)
    {
      return obj.GetType() == GetType() && ((MaterialProperty)obj).Equals(this);
    }

    public override string ToString()
    {
      return String.Format("{0}={1}{2}", ShortName, Value, Unit);
    }

    public override int GetHashCode()
    {
      return ShortName.GetHashCode();
    }
  }
}
