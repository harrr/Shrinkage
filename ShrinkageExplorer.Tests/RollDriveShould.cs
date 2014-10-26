using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ShrinkageExplorer.Core;
using ShrinkageExplorer.Core.DataClasses;

namespace ShrinkageExplorer.Tests
{
  [TestFixture]
  class RollDriveShould
  {
    private RollDrive _rollDrive;
    

    [SetUp]
    public void Setup()
    {
      _rollDrive = new RollDrive(new[]
      {
        new GeometryRoll(0, 0, 10),
        new GeometryRoll(30,0,10), 
        new GeometryRoll(50,0,10)
      });
    }
    [Test]
    [ExpectedException(typeof(ShrinkageExplorerException))]
    public void ThrowExceptionWhenAddCrossingRoll()
    {
      _rollDrive.AddRoll(new GeometryRoll(20,0,10));

      Assert.AreEqual(_rollDrive.WorkingRolls.Count(),3);
    }

    [Test]
    public void AddRollWhenAddCorrectRoll()
    {
      var newRoll = new GeometryRoll(50, 100, 10);

      _rollDrive.AddRoll(newRoll);

      Assert.AreEqual(_rollDrive.WorkingRolls.Count(), 4);
      Assert.IsTrue(_rollDrive.WorkingRolls.ElementAt(3).Equals(newRoll));
    }

    [Test]
    [ExpectedException(typeof(ShrinkageExplorerException))]
    public void ThrowExceptionWhenRemoveNotExistingRoll()
    {
      _rollDrive.RemoveRoll(new GeometryRoll(10,10,10));
    }
  }
}
