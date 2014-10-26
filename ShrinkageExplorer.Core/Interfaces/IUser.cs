
namespace ShrinkageExplorer.Core.Interfaces
{
  public enum UserTypes
  {
    Operator,
    Administrator
  }

  public interface IUser
  {
    string Name { get; }
    UserTypes UserType { get; }
    string CryptedPassword { get; set; }

    bool CheckPassword(string password);
  }
}
