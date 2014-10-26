using System.Collections.Generic;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Core.Repository;

namespace ShrinkageExplorer.Data
{
  public class PropertiesRepository : IPropertiesRepository
  {
    private readonly ShrinkageEntities _entities;

    public PropertiesRepository(ShrinkageEntities entities)
    {
      _entities = entities;

      Properties = _entities.Parameters.Local;
    }


    public IEnumerable<IProperty> Properties { get; private set; }

    #region Члены IPropertiesRepository


    public void AddNew()
    {
      _entities.Parameters.Local.Add(new Parameter
      {
        Name = "",
        ShortName = "",
        Unit = ""
      });
    }


    #endregion
  }
}
