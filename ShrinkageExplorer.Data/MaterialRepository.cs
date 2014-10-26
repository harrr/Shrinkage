using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Core.Repository;

namespace ShrinkageExplorer.Data
{
  public class MaterialRepository : IMaterialsRepository
  {
    private readonly ShrinkageEntities _entities;

    public IEnumerable<IMaterial> Materials { get; private set; }
    public MaterialRepository(ShrinkageEntities entities)
    {
      _entities = entities;

      Materials = _entities.Materials.Local;
    }


    #region Члены IMaterialsRepository


    public void AddNew()
    {
      _entities.Materials.Add(new Material());
    }

    #endregion
  }
}
