using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using ShrinkageExplorer.Core.Interfaces;
using System.Xml;
using System.IO;
using System.Text;


namespace ShrinkageExplorer.Data
{
  public static class LineLoader
  {
    public static IRollLine Load(string fileName)
    {
        Line line;
        line = new Line();
        IRollLine loadedLine = line.Clone();
        //IRollLine line;
        int drivesCount = 0;
        try 
        {
            XmlDocument document = new XmlDocument();
            document.Load(fileName);
            var lineNode = document.DocumentElement.SelectSingleNode("/line");
            line.AirCoefficient = float.Parse(lineNode.Attributes["airCoefficient"].InnerText);
            line.AirTemperature = float.Parse(lineNode.Attributes["airTemperature"].InnerText);
            line.RollCoefficient = float.Parse(lineNode.Attributes["rollCoefficient"].InnerText);
            foreach (XmlNode driveNode in lineNode.ChildNodes)
            {
                Drive drive = new Drive();
                drive.Number = int.Parse(driveNode.Attributes["number"].InnerText);
                drive.Temperature = float.Parse(driveNode.Attributes["temperature"].InnerText);
                drive.Velocity = float.Parse(driveNode.Attributes["velocity"].InnerText);
                drive.MinTemperature = float.Parse(driveNode.Attributes["min_temperature"].InnerText);
                drive.MaxTemperature = float.Parse(driveNode.Attributes["max_temperature"].InnerText);
                drive.MinVelocity = float.Parse(driveNode.Attributes["min_velocity"].InnerText);
                drive.MaxVelocity = float.Parse(driveNode.Attributes["max_velocity"].InnerText);
                foreach (XmlNode rollNode in driveNode.ChildNodes)
                {
                    Roll roll = new Roll();
                    roll.X = float.Parse(rollNode.Attributes["x"].InnerText);
                    roll.Y = float.Parse(rollNode.Attributes["y"].InnerText);
                    roll.Radius = float.Parse(rollNode.Attributes["radius"].InnerText);
                    roll.Clockwise = bool.Parse(rollNode.Attributes["clockwise"].InnerText);
                    drive.GeometryRolls.Add(roll.Clone());
                }
                line.Drives.Add(drive);
            }
            return line.Clone();
        }
        catch (Exception Ex)
        {
            return null;
        }
    }
  }
}
