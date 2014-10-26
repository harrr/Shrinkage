using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ShrinkageExplorer.Core;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Data
{
  public partial class User : IUser
  {
    private static readonly MD5 Hasher;

    static User()
    {
      Hasher = MD5.Create();
    }

    public UserTypes UserType
    {
      get
      {
        return isAdmin ? UserTypes.Administrator : UserTypes.Operator;
      }
    }


    public bool CheckPassword(string password)
    {
      string pass = Encoding.UTF8.GetString(Hasher.ComputeHash(Encoding.Default.GetBytes(password)));
      return Password == pass;
    }

    public string CryptedPassword
    {
      get { return Password; }
      set
      {
        if (String.IsNullOrWhiteSpace(value))
          return;
        Password = Encoding.UTF8.GetString(Hasher.ComputeHash(Encoding.Default.GetBytes(value)));
      }
    }
  }
}
