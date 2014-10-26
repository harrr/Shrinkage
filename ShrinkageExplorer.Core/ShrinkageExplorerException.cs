using System;

namespace ShrinkageExplorer.Core
{
  public class ShrinkageExplorerException : Exception
  {
    public ShrinkageExplorerException()
    {
    }

    public ShrinkageExplorerException(string message)
      : base(message)
    {
    }
  }
}