using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ThemeParkSimulator.XMLObjects
{
    [XmlRoot()]
    public class AttractionsXMLDataObject
    {
        private List<Attraction> attractions;

        public AttractionsXMLDataObject() {
            Attractions = new List<Attraction>();
        }

        public List<Attraction> Attractions
        {
            get { return attractions; }
            set { attractions = value; }
        }
    }

    public class Attraction {
        private int id;
        private string name;
        private string description;
        private double cost;
        private int happiness;
        private int nausia;
        private int attractiveness;

        [XmlAttribute]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [XmlAttribute]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlAttribute]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [XmlAttribute]
        public double Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        [XmlAttribute]
        public int Happiness
        {
            get { return happiness; }
            set { happiness = value; }
        }

        [XmlAttribute]
        public int Nausia
        {
            get { return nausia; }
            set { nausia = value; }
        }

        [XmlAttribute]
        public int Attractiveness
        {
            get { return attractiveness; }
            set { attractiveness = value; }
        }
    }
}
