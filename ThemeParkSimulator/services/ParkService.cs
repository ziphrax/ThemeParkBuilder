using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ThemeParkSimulator.models;
using ThemeParkSimulator.XMLDataObjects;

namespace ThemeParkSimulator.services
{
    public interface IParkService
    {
        ParksXMLDataObject GetParks();        
        ParkModel GetPark(int Id);
        void SaveParks(ParksXMLDataObject parkData);
    }

    public class ParkService : IParkService
    {

        public ParksXMLDataObject GetParks()
        {
            string xml = @File.ReadAllText("./data/parks.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(ParksXMLDataObject));

            return (ParksXMLDataObject)serializer.Deserialize(new StringReader(xml));
        }

        public void SaveParks(ParksXMLDataObject parkData) {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ParksXMLDataObject));
                string path = "./data/parks.xml";
                FileStream file = File.Create(path);
                serializer.Serialize(file, parkData);
                file.Close();
            }
            catch (Exception e) {
                Console.WriteLine("Could not save file :-(");
            }           
           
        }

        public ParkModel GetPark(int Id)
        {
            return new ParkModel();
        }
    }
}
