using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ThemeParkSimulator.XMLObjects;

namespace ThemeParkSimulator.XMLDataObjects
{
    [XmlRoot()]
    public class ParksXMLDataObject
    {
        [XmlIgnore]
        private List<Park> parks;

        public ParksXMLDataObject()
        {
            Parks = new List<Park>();
        }

        public List<Park> Parks
        {
            get { return parks; }
            set { parks = value; }
        }

    }

    public class Park {
        [XmlAttribute]
        public double CurrentBalance { get; set; }

        [XmlAttribute]
        public double TotalSpent { get; set; }

        [XmlAttribute]
        public double TotalEarnt { get; set; }

        [XmlAttribute]
        public Int32 ActionCount { get; set; }

        [XmlAttribute]
        public DateTime CreatedDate { get; set; }

        [XmlAttribute]
        public Int32 CurrentVisitors { get; set; }

        [XmlAttribute]
        public long CurrentTicks { get; set; }

        [XmlAttribute]
        public String ParkName { get; set; }

        public List<Attraction> BuiltAttractions { get; set; }

    }
}
