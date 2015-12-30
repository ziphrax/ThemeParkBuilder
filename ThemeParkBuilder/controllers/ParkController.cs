using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThemeParkBuilder.services;
using ThemeParkBuilder.XMLDataObjects;

namespace ThemeParkBuilder.controllers
{
    public class ParkController
    {
        private IParkService parkService;

        public ParkController()
        {
            this.parkService = new ParkService();
        }

        public ParkController(IParkService parkService)
        {
            this.parkService = parkService;
        }

        public ParksXMLDataObject GetParks()
        {
            return parkService.GetParks();
        }

        public void SaveParks(ParksXMLDataObject parkData) {
            parkService.SaveParks(parkData);
        }
    }
}
