using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Project_3
{
    public class Configuration
    {
        public int NumOfLocations { get; private set; }
        public int MeanPop { get; private set; }
        public int PopDeviation { get; private set; }
        public double SpreadChance { get; private set; }
        public double DeathChance { get; private set; }
        public int DiseaseDuration { get; private set; }
        public int QuarantineDuration { get; private set; }
        public double QuarantineChanceMean { get; private set; }
        public double QuarantineChanceDeviation { get; private set; }
        public int SimDuration { get; private set; }
        public double TravelChance { get; private set; }
        
        public Configuration()
        {
            XDocument doc = XDocument.Load("..\\..\\..\\..\\Project 3\\project 3.config");

            NumOfLocations = int.Parse((string)doc.XPathSelectElement("/configuration/appSettings/*[1]"));
            MeanPop = int.Parse((string)doc.XPathSelectElement("/configuration/appSettings/*[2]"));
            PopDeviation = int.Parse((string)doc.XPathSelectElement("/configuration/appSettings/*[3]"));
            SpreadChance = double.Parse((string)doc.XPathSelectElement("/configuration/appSettings/*[4]"));
            DeathChance = double.Parse((string)doc.XPathSelectElement("/configuration/appSettings/*[5]"));
            DiseaseDuration = int.Parse((string)doc.XPathSelectElement("/configuration/appSettings/*[6]"));
            QuarantineDuration = int.Parse((string)doc.XPathSelectElement("/configuration/appSettings/*[7]"));
            QuarantineChanceMean = double.Parse((string)doc.XPathSelectElement("/configuration/appSettings/*[8]"));
            QuarantineChanceDeviation = double.Parse((string)doc.XPathSelectElement("/configuration/appSettings/*[9]"));
            SimDuration = int.Parse((string)doc.XPathSelectElement("/configuration/appSettings/*[10]"));
            TravelChance = double.Parse((string)doc.XPathSelectElement("/configuration/appSettings/*[11]"));
        }
        
    }
}
