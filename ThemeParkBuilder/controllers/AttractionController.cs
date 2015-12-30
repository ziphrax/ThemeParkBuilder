using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThemeParkBuilder.services;
using ThemeParkBuilder.XMLObjects;

namespace ThemeParkBuilder.controllers
{
    public class AttractionController
    {
        private IAttractionService attractionService;

        public AttractionController()
        {
            this.attractionService = new AttractionService();
        }

        public AttractionController(IAttractionService attractionService) {
            this.attractionService = attractionService;
        }

        public AttractionsXMLDataObject GetAttractions() {
            return attractionService.GetAttractions();
        }
    }
}
