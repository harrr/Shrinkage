using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShrinkageExplorer.Core;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Data
{
  public partial class MaterialParameter : IMaterialProperty
  {
    public MaterialParameter()
    {
      
    }
    public MaterialParameter(Parameter property)
    {
      Parameter = property;
      Value = "0";
    }

    #region Члены IMaterialProperty

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
        var arrValue = new double[strValues.Length];
        for (int i = 0; i < strValues.Length; ++i)
          arrValue[i] = Double.Parse(strValues[i]);
        return arrValue;
      }
    }
    #endregion

    #region Члены IProperty

    public string Name
    {
      get { return Parameter.Name; }
      set { Parameter.Name = value; }
    }

    public string ShortName
    {
      get { return Parameter.ShortName; }
      set { Parameter.ShortName = value; }
    }

    public string Unit
    {
      get { return Parameter.Unit; }
      set { Parameter.Unit = value; }
    }

    #endregion

    public bool Equals(IProperty other)
    {
      if (Parameter == null)
        return false;
      return String.Equals(ShortName, other.ShortName);
    }

    public bool Equals(IMaterialProperty other)
    {
      return Equals(other as IProperty) && String.Equals(Value, other.Value);
    }

    public override bool Equals(object obj)
    {
      return obj.GetType() == GetType() && ((MaterialParameter)obj).Equals(this);
    }

    public override string ToString()
    {
      return String.Format("{0}={1}{2}", ShortName, Value, Unit);
    }

    public override int GetHashCode()
    {
      if (Parameter == null)
        return -1;
      return ShortName.GetHashCode();
    }


  }
}
