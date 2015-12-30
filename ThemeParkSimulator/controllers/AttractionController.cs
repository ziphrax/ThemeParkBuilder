using ThemeParkSimulator.services;
using ThemeParkSimulator.XMLObjects;

namespace ThemeParkSimulator.controllers
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
