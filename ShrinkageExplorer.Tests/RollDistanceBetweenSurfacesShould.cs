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
  class RollDistanceBetweenSurfacesShould
  {
    private IGeometryRoll _roll;
    [SetUp]
    public void Setup()
    {
      _roll = new IGeometryRoll(0,0,50);
    }

    [Test]
    public void ShouldReturnNegativeWhenActingWithSameRoll()
    {
      Assert.True(_roll.GetDistanceBetweenSurfacesWith(_roll) < 0);
    }

    [Test]
    public void ShouldReturnWhenActingWithCrossingRoll()
    {
      var crossingRoll = new IGeometryRoll(20, 0, 50);
      Assert.True(_roll.GetDistanceBetweenSurfacesWith(crossingRoll) < 0);
    }

    [Test]
    public void ShouldReturnZeroWhenActingWithJoinedRoll()
    {
      var joinedRoll = new IGeometryRoll(100, 0, 50);
      Assert.AreEqual(_roll.GetDistanceBetweenSurfacesWith(joinedRoll), 0);
    }

    [Test]
    public void ShouldReturn50WhenActingWithRollWhichIs150HorizontalAnd50Radius()
    {
      var anotherRoll = new IGeometryRoll(150, 0, 50);
      Assert.AreEqual(_roll.GetDistanceBetweenSurfacesWith(anotherRoll),50);
    }
  }
}
