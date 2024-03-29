//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Data
{
  using System;
  using System.Collections.Generic;

  public partial class Drive
  {
    public Drive()
    {
      this.Rolls = new ObservableCollection<IGeometryRoll>();
    }

    public int Id { get; set; }
    public float Velocity { get; set; }
    public float Temperature { get; set; }
    public Nullable<int> LineId { get; set; }
    public float MinVelocity { get; set; }
    public float MaxVelocity { get; set; }
    public float MinTemperature { get; set; }
    public float MaxTemperature { get; set; }
    public int Number { get; set; }

    public virtual Line Line { get; set; }
    public virtual ICollection<IGeometryRoll> Rolls { get; set; }
  }
}
