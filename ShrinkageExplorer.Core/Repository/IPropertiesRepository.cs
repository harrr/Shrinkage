using System.Collections.Generic;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.Repository
{
  public interface IPropertiesRepository
  {
    IEnumerable<IProperty> Properties { get; }
    
    void AddNew();
  }
}
