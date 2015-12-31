using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ThemeParkSimulator;
using ThemeParkSimulator.controllers;
using ThemeParkSimulator.XMLDataObjects;
using ThemeParkSimulator.XMLObjects;

namespace ThemeParkBuilder
{
    class Program
    {
        static double StartingMoney = 2000.00;      
        static bool running = true;

        static AttractionsXMLDataObject attractionsData;
        static ParksXMLDataObject parksData;
        static Park selectedPark;

        static Simulator Simulation;        

        //controllers
        static AttractionController attractionController;
        static ParkController parkController;

        static void Main(string[] args)
        {
            Console.WriteLine("Initilising Application...");
            setupControllers();
            readRequiredData();
            Console.Clear();

            gameStartedMessage();           

            string NextAction = "";

            Console.WriteLine("Type 'List Actions' to view available actions.");

            while (running) {
                Console.WriteLine("");
                Console.WriteLine("What would you like to do?");
                NextAction = Console.ReadLine();
                performAction(NextAction);
            }

            closeGame();
        }

        static void startSimulation() {
            if (selectedPark != null)
            {
                Console.WriteLine("Starting simulation...");
                Simulation = new Simulator(ref selectedPark);
                Simulation.Start();              
            }
            else {
                Console.WriteLine("You must first select a park");
            }            
        }

        static void stopSimulation() {
            if (selectedPark != null)
            {
                Console.WriteLine("Stopping simulation...");
                Simulation.RequestStop();
                while (Simulation.IsRunning) ;
                Console.WriteLine("Simulation has stopped!");
            }
        }

        static void gameStartedMessage() {
            Console.WriteLine("Theme Park Buiilder - By David Jones");
            Console.WriteLine("-------------------------");
            Console.WriteLine("");
            Console.WriteLine("Welcome to theme park builder.");
        }

        static void newGameMessage() {            
            Console.WriteLine("What would you like to call your new theme park?");
            selectedPark = new Park();
            selectedPark.ParkName = Console.ReadLine();
        }

        static void successNewGameCreated() {
            selectedPark.BuiltAttractions= new List<Attraction>();
            selectedPark.CreatedDate = DateTime.Now;
            Console.WriteLine("Great! you have named your new park '" + selectedPark.ParkName + "'. You start with £ " + StartingMoney + ". Type 'List Actions' to see a list of available actions.");
            selectedPark.CurrentBalance = StartingMoney;            
        }

        static void readRequiredData() {
            readAttractionsData();
            readParksData();
        }

        static void readAttractionsData() {
            Console.WriteLine("Loading attraction data...a");
            attractionsData = attractionController.GetAttractions();
            Console.WriteLine("Done!");
        }

        static void readParksData() {
            Console.WriteLine("Loading park data...");
            parksData = parkController.GetParks();            
            Console.WriteLine("Done!");
        }

        static void saveParksData() {
            //There must be a way to update this data instead?
            if (selectedPark != null) {
                var removeItems = parksData.Parks.SingleOrDefault<Park>(s => s.ParkName == selectedPark.ParkName);
                parksData.Parks.Remove(removeItems);
                parksData.Parks.Add(selectedPark);
            }
            
            parkController.SaveParks(parksData);
        }

        //I really should rewrite this...
        static void performAction(String action) {
            switch (action) {                
                case "List Actions":                    
                    listAvailableActions();
                    break;
                case "View Parks":
                    if (parksData == null) {
                        parksData = new ParksXMLDataObject();
                    }
                    viewParks(parksData.Parks);
                    break;
                case "Load Parks":
                    readParksData();
                    break;
                case "Save Parks":
                    saveParksData();
                    break;
                case "Select Park":
                    selectPark();
                    break;
                case "Create New Park":
                    newGameMessage();
                    successNewGameCreated();
                    break;
                case "Start Simulation":
                    startSimulation();
                    break;
                case "Stop Simulation":
                    stopSimulation();
                    break;
                case "View Park Stats":
                    increaseActionCount();
                    viewParkStats();
                    break;
                case "View Available Attractions":
                    increaseActionCount();
                    viewAttractions( attractionsData.Attractions );
                    break;
                case "View Built Attractions":
                    increaseActionCount();
                    viewAttractions(selectedPark.BuiltAttractions);
                    break;
                case "Build Attraction":
                    increaseActionCount();
                    buildAttractionCommand();
                    break;
                case "Reload Attractions Data":
                    increaseActionCount();
                    readAttractionsData();
                    break;
                case "Clear Console":
                    Console.Clear();
                    break;
                case "Quit":
                    stopSimulation();
                    running = false;
                    break;
                default:
                    Console.WriteLine("'" + action + "' is an unrecognised command. Please try again or type 'List Actions' to see a list of available actions.");
                    break;
            }
        }

        static void increaseActionCount() {
            if (selectedPark != null) {
                selectedPark.ActionCount++;
            }            
        }

        static void viewParkStats() {
            Console.WriteLine("");
            Console.WriteLine("=========================");
            Console.WriteLine("Park Stats");
            Console.WriteLine("=========================");
            Console.WriteLine("");
            Console.WriteLine("Park Name: " + selectedPark.ParkName);
            Console.WriteLine("Created: " + selectedPark.CreatedDate);
            Console.WriteLine("Current Balance: £ " + selectedPark.CurrentBalance);
            Console.WriteLine("Ticket Price: £ " + selectedPark.TicketPrice);
            Console.WriteLine("Current Ticks: " + selectedPark.CurrentTicks);
            Console.WriteLine("Total Spent: £ " + selectedPark.TotalSpent);
            Console.WriteLine("Total Earnt: £ " + selectedPark.TotalEarnt);
            Console.WriteLine("Current Visitor Count: " + selectedPark.CurrentVisitors);
            Console.WriteLine("Built Attractions: " + selectedPark.BuiltAttractions.Count);
            Console.WriteLine("Spent Actions: " + selectedPark.ActionCount);
            Console.WriteLine("Current Rating: " + selectedPark.CurrentRating);
        }

        static void listAvailableActions() {
            //How to map these better to user input?
            Console.WriteLine("");
            Console.WriteLine("Here are your available actions:");            
            Console.WriteLine("List Actions");
            Console.WriteLine("View Parks");
            Console.WriteLine("Load Parks");
            Console.WriteLine("Save Parks");
            Console.WriteLine("Create New Park");
            Console.WriteLine("Select Park");
            Console.WriteLine("Start Simulation");
            Console.WriteLine("Stop Simulation");
            Console.WriteLine("View Park Stats");
            Console.WriteLine("View Available Attractions");
            Console.WriteLine("View Built Attractions");
            Console.WriteLine("Build Attraction");
            Console.WriteLine("Reload Attractions Data");
            Console.WriteLine("Clear Console");
            Console.WriteLine("Quit");       
        }

        static void viewParks(List<Park> arr_parks) {
            Console.WriteLine("");
            Console.WriteLine("========================");
            Console.WriteLine("Parks");
            Console.WriteLine("========================");

            foreach (Park a in arr_parks)
            {
                Console.WriteLine("Name: " + a.ParkName + ", Current Balance: £" + a.CurrentBalance);
                Console.WriteLine("Current Visitors: " + a.CurrentVisitors);
                Console.WriteLine("Attractions Built: " + a.BuiltAttractions.Count);
                Console.WriteLine("========================");
            }

            if (arr_parks.Count == 0)
            {
                Console.WriteLine("There are no parks to view.");
            }
        }

        static void viewAttractions(List<Attraction> arr_attractions) {
            Console.WriteLine("");
            Console.WriteLine("========================");
            Console.WriteLine("Attractions");
            Console.WriteLine("========================");

            foreach (Attraction a in arr_attractions) {
                Console.WriteLine("");
                Console.WriteLine("Id: " + a.Id + " , Name: " + a.Name + ", Cost: £" + a.Cost);
                Console.WriteLine("Descriptions: " + a.Description);
                Console.WriteLine("Attractiveness: " + a.Attractiveness);
                Console.WriteLine("Happiness: " + a.Happiness);
                Console.WriteLine("Nausia: " + a.Nausia);                
                Console.WriteLine("========================");
            }

            if (arr_attractions.Count == 0) {
                Console.WriteLine("There are no attractions to view.");
            }

        }

        static void buildAttractionCommand() {
            Console.WriteLine();
            Console.WriteLine("Which attraction would you like to build?");
            Int32 filterID = Int32.Parse(Console.ReadLine());
            Attraction selectedAttraction = (from a in attractionsData.Attractions
                         where a.Id == filterID
                         select a).First<Attraction>();

            if (selectedPark.CurrentBalance - selectedAttraction.Cost > 0)
            {
                buildAttractionSuccess(selectedAttraction);
            }
            else {
                buildAttractionFailed(selectedAttraction);
            }            
        }

        static void buildAttractionSuccess(Attraction selectedAttraction) {
            selectedPark.BuiltAttractions.Add(selectedAttraction);
            selectedPark.CurrentBalance -= selectedAttraction.Cost;
            selectedPark.TotalSpent += selectedAttraction.Cost;
            Console.WriteLine("You have successfully built a new " + selectedAttraction.Name + ". You have £ " + selectedPark.CurrentBalance + " remaining.");           
        }

        static void buildAttractionFailed(Attraction selectedAttraction)
        {
            Console.WriteLine("You cannot build a " + selectedAttraction.Name + ". You do not have enough money. You only have £ " + selectedPark.CurrentBalance);
        }

        static void selectPark() {
            Console.WriteLine("Which park would you like to load?");
            String filterId = Console.ReadLine();
            selectedPark = (from p in parksData.Parks
                            where p.ParkName == filterId
                            select p).First<Park>();            
        }

        static void setupControllers() {
            Console.WriteLine("Setting up controllers...");
            attractionController = new AttractionController();
            parkController = new ParkController();
            Console.WriteLine("Done!");
        }

        static void closeGame() {            
            Console.WriteLine("Thank you for playing Theme Park Builder. Press return to close the game.");
            Console.ReadLine();            
        }
    }
}
