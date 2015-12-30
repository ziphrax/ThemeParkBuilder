using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ThemeParkSimulator.models;
using ThemeParkSimulator.XMLObjects;

namespace ThemeParkSimulator.services
{
    public interface IAttractionService
    {
        AttractionsXMLDataObject GetAttractions();        
        AttractionModel GetAttraction(int Id);
        void SaveAttractions(AttractionsXMLDataObject attractionData);
    }

    public class AttractionService : IAttractionService{

        public AttractionsXMLDataObject GetAttractions() {
            string xml = @File.ReadAllText("./data/attractions.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(AttractionsXMLDataObject));

            return (AttractionsXMLDataObject)serializer.Deserialize(new StringReader(xml));
        }

        public void SaveAttractions(AttractionsXMLDataObject attractionData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AttractionsXMLDataObject));
            string path = "./data/attractions.xml";
            FileStream file = File.Create(path);
            serializer.Serialize(file, attractionData);
            file.Close();
        }

        public AttractionModel GetAttraction(int Id){
            return new AttractionModel();
        }
    }
}
