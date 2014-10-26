using System.Globalization;
using System.Xml.Linq;
using ShrinkageExplorer.Core.Interfaces;
using System;
using System.Data;
using System.Xml; 

namespace ShrinkageExplorer.Data
{
  public class LineSaver
  {
    private readonly IRollLine _line;
    private readonly string _fileName;

    public LineSaver(IRollLine line, string fileName)
    {
      _line = line;
      _fileName = fileName;
    }

    public void Save()
    {
        XmlTextWriter writer = new XmlTextWriter(this._fileName, System.Text.Encoding.UTF8);
        writer.WriteStartDocument(true);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 2;
        writer.WriteStartElement("line");
        writer.WriteAttributeString("airCoefficient", _line.AirCoefficient.ToString());
        writer.WriteAttributeString("rollCoefficient", _line.RollCoefficient.ToString());
        writer.WriteAttributeString("airTemperature", _line.AirTemperature.ToString());
        for (int i = 0; i < _line.Drives.Count; i++)
        {
            createDriveNode(writer, _line.Drives[i]);
        }
        /*createNode("1", "Product 1", "1000", writer);
        createNode("2", "Product 2", "2000", writer);
        createNode("3", "Product 3", "3000", writer);
        createNode("4", "Product 4", "4000", writer);*/
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Close();
        //MessageBox.Show("XML File created ! ");
    }

    private void createDriveNode(XmlTextWriter writer, IRollDrive drive)
    {
        writer.WriteStartElement("drive");
        writer.WriteAttributeString("number", drive.Number.ToString());
        writer.WriteAttributeString("temperature", drive.Temperature.ToString());
        writer.WriteAttributeString("velocity", drive.Velocity.ToString());
        writer.WriteAttributeString("min_temperature", drive.MinTemperature.ToString());
        writer.WriteAttributeString("max_temperature", drive.MaxTemperature.ToString());
        writer.WriteAttributeString("min_velocity", drive.MinVelocity.ToString());
        writer.WriteAttributeString("max_velocity", drive.MaxVelocity.ToString());
        for (int i = 0; i < drive.WorkingRolls.Count; i++)
        {
            createRollNode(writer, drive.WorkingRolls[i]);
        }
        writer.WriteEndElement();
    }

    private void createRollNode(XmlTextWriter writer, IWorkingRoll roll)
    {
        writer.WriteStartElement("roll");
        writer.WriteAttributeString("x", roll.X.ToString());
        writer.WriteAttributeString("y", roll.Y.ToString());
        writer.WriteAttributeString("radius", roll.Radius.ToString());
        writer.WriteAttributeString("clockwise", roll.Clockwise.ToString());
        writer.WriteEndElement();
    }
  }
}