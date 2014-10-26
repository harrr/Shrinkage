using System;
using System.Collections.Generic;
using ShrinkageExplorer.Core.Models;

namespace ShrinkageExplorer.Core.Interfaces
{
  public interface IModel : IEquatable<IModel>
  {
    ShrinkageModel MathModel { get; }
    string ClassName { get; set; }
    string Name { get; set; }
    float? AvgError { get; set; }

    ICollection<IProperty> RequiredProperties { get; }
  }
}
