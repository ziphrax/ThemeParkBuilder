using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThemeParkBuilder.models
{
    public class AttractionModel
    {
        int Id { get; }
        string Name { get; set; }
        string Description { get; set; }

        double Cost { get; set; }

        double Attractivnes { get; set; }
        double Excitment { get; set; }
        double Nausia { get; set; }
        
    }
}
