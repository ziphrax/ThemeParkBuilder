using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThemeParkBuilder.XMLObjects;

namespace ThemeParkBuilder.models
{
    public class ParkModel
    {
        double CurrentBalance { get; set; }
        double TotalSpent { get; set; }
        double TotalEarnt { get; set; }
        Int32 ActionCount { get; set; }
        DateTime CreatedDate { get; set; }
        Int32 CurrentVisitors { get; set; }
        String ParkName { get; set; }
        List<Attraction> BuiltAttractions { get; set; }
    }
}
