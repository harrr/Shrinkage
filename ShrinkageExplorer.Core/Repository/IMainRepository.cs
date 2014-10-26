
namespace ShrinkageExplorer.Core.Repository
{
  public interface IMainRepository
  {
    ILinesRepository LinesRepository { get; }
    IMaterialsRepository MaterialsRepository { get; }
    IPropertiesRepository PropertiesRepository { get; }
    IModelsRepository ModelsRepository { get; }
    IUsersRepository UsersRepository { get; }

    void SaveChanges();
  }
}
