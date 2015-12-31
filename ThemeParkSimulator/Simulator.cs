using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThemeParkSimulator.XMLDataObjects;
using ThemeParkSimulator.XMLObjects;

namespace ThemeParkSimulator
{
    public class Simulator
    {
        public int VisitorArrivalInterval = 500;
        public double LeavingRate = 0.9;
        public volatile bool IsRunning;
        public AccurateTimer Timer { get; set; }
        private Park parkData;
        private int frameCount = 0;

        public Simulator(ref Park parkData) {
            this.IsRunning = false;
            this.Timer = new AccurateTimer();
            this.parkData = parkData;
        }

        public void GameLoop() {
            while (IsRunning) {
                Timer.Reset();
                Update(ref parkData);
                Thread.Sleep(50);
            }
            Console.WriteLine("Simulation stopped gracefully.");
        }

        public void Start() {
            this.IsRunning = true;
            Thread simulationThread = new Thread(GameLoop);
            simulationThread.Start();
        }

        public void RequestStop() {
            IsRunning = false;
        }

        public void Update(ref Park parkData) {
            parkData.CurrentTicks = frameCount++;
            UpdateVisitorCount();
            parkData.CurrentRating = CalculateExperienceRating();
        }

        public void UpdateVisitorCount() {
            if (parkData.CurrentTicks > VisitorArrivalInterval)
            {
                frameCount = 0;
                parkData.CurrentVisitors = CalcLeavingVisitors();
                Int32 newVisitors = CalcNewVisitors();
                SumEntranceTicketSales(newVisitors);
            }
        }

        public void SumEntranceTicketSales(Int32 newVisitors) {
            parkData.CurrentVisitors += newVisitors;
            parkData.CurrentBalance += newVisitors * parkData.TicketPrice;
        }

        public Int32 CalcLeavingVisitors() {
            return (int)(parkData.CurrentVisitors * LeavingRate); 
        }

        public Int32 CalcNewVisitors() {
            int ticketBoothCount = (
                from tb in parkData.BuiltAttractions
                where tb.Name == "Ticket Booth"
                select tb
                ).Count<Attraction>();

            var basicAmount = new Random().Next(1, 20);

            if (basicAmount < ticketBoothCount * 50)
            {
                return basicAmount;
            }
            else {
                return ticketBoothCount * 50;
            }            
        }

        public double CalculateExperienceRating() {
            var actual_attractions = from eachAttraction in parkData.BuiltAttractions
                                    where eachAttraction.Name != "Ticket Booth"
                                    select eachAttraction;

            double total_attraction = actual_attractions.Sum<Attraction>(x => x.Attractiveness);
            double total_happiness = actual_attractions.Sum<Attraction>(x => x.Happiness);
            double total_nausia = actual_attractions.Sum<Attraction>(x => x.Nausia);

            double[] attraction_visitors = new double[actual_attractions.Count<Attraction>()];
            double[] attraction_happiness = new double[actual_attractions.Count<Attraction>()];
            double[] attraction_nausia = new double[actual_attractions.Count<Attraction>()];
            double[] attraction_ratings = new double[actual_attractions.Count<Attraction>()];

            int index = 0;
            foreach (Attraction ea in actual_attractions) {
                double percentage = ea.Attractiveness / total_attraction;
                attraction_visitors[index] = parkData.CurrentVisitors * percentage ;
                attraction_happiness[index] = ea.Happiness * attraction_visitors[index];
                attraction_nausia[index] = -ea.Nausia * attraction_visitors[index];

                attraction_ratings[index] = attraction_happiness[index] - attraction_nausia[index];

                index++;
            }

            return attraction_ratings.Sum() / (parkData.CurrentVisitors * 100) * 5;
        }

    }

    public class AccurateTimer
    {
        [DllImport("kernel32.dll")]
        private static extern long GetTickCount();

        private long StartTick = 0;

        public AccurateTimer()
        {
            Reset();
        }

        public void Reset()
        {
            StartTick = GetTickCount();
        }

        public long GetTicks()
        {
            long currentTick = 0;
            currentTick = GetTickCount();

            return currentTick - StartTick;
        }

    }

}
