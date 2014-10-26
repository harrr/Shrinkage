using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using ShrinkageExplorer.Core.Repository;

namespace ShrinkageExplorer.Data
{
  public class MainRepository : IMainRepository
  {
    private ShrinkageEntities _entities;
    #region Члены IMainRepository

    public ILinesRepository LinesRepository { get; private set; }

    public IMaterialsRepository MaterialsRepository { get; private set; }

    public IPropertiesRepository PropertiesRepository { get; private set; }

    public IModelsRepository ModelsRepository { get; private set; }

    public IUsersRepository UsersRepository { get; private set; }

    #endregion

    public MainRepository()
    {
      _entities = new ShrinkageEntities();
      _entities.Drives.Load();
      _entities.Lines.Load();
      _entities.MaterialParameters.Load();
      _entities.Materials.Load();
      _entities.Models.Load();
      _entities.Parameters.Load();
      _entities.Rolls.Load();
      _entities.Users.Load();

      LinesRepository = new LineRepository(_entities);
      MaterialsRepository = new MaterialRepository(_entities);
      PropertiesRepository = new PropertiesRepository(_entities);
      ModelsRepository = new ModelRepository(_entities);
      UsersRepository = new UsersRepository(_entities);
    }

    public void SaveChanges()
    {
      _entities.SaveChanges();
    }
  }
}
