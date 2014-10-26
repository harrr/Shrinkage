using System;
using NUnit.Framework;
using ShrinkageExplorer.Core;
using ShrinkageExplorer.Core.DataClasses;

namespace ShrinkageExplorer.Tests
{
  [TestFixture]
  public class RollDistanceBetweenCentersShould
  {
    private IGeometryRoll _roll;
    [SetUp]
    public void Setup()
    {
      _roll = new IGeometryRoll(0,0,50);
    }
    [Test]
    public void ShouldReturnZeroWhenActingWithSameRoll()
    {
      Assert.AreEqual(_roll.GetDistanceBetweenCentersWith(_roll), 0);
    }

    [Test]
    public void ShouldReturn50WhenActingWithRollWhichIs50Horizontal()
    {
      var anotherRoll = new IGeometryRoll(50, 0, 0);
      Assert.AreEqual(_roll.GetDistanceBetweenCentersWith(anotherRoll), 50);
    }

    [Test]
    public void ShouldReturn50WhenActingWithRollWhichIs50Vertical()
    {
      var anotherRoll = new IGeometryRoll(0, 50, 0);
      Assert.AreEqual(_roll.GetDistanceBetweenCentersWith(anotherRoll), 50);
    }

    [Test]
    public void ShouldReturnSqrt200WhenActingWithRollWhichIs10HorizontalAnd10Vertical()
    {
      var anotherRoll = new IGeometryRoll(10, 10, 0);
      Assert.AreEqual(_roll.GetDistanceBetweenCentersWith(anotherRoll), Math.Sqrt(200),0.001);
    }

  }
}