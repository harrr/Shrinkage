using System.Collections.Generic;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Core.Repository;

namespace ShrinkageExplorer.Data
{
  public class ModelRepository : IModelsRepository
  {
    private readonly ShrinkageEntities _entities;


    public IEnumerable<IModel> Models { get; private set; }



    public ModelRepository(ShrinkageEntities entities)
    {
      _entities = entities;

      Models = _entities.Models.Local;
    }

    public void AddNew()
    {
      _entities.Models.Local.Add(new Model());
    }

    public void SaveChanges()
    {
      _entities.SaveChanges();
    }
  }
}
