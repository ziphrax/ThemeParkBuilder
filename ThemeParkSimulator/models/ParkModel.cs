using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThemeParkSimulator.XMLObjects;

namespace ThemeParkSimulator.models
{
    public class ParkModel
    {
        double CurrentBalance { get; set; }
        double TotalSpent { get; set; }
        double TotalEarnt { get; set; }
        Int32 ActionCount { get; set; }
        long CurrentTicks { get; set; }
        DateTime CreatedDate { get; set; }
        Int32 CurrentVisitors { get; set; }
        String ParkName { get; set; }
        List<Attraction> BuiltAttractions { get; set; }
    }
}
