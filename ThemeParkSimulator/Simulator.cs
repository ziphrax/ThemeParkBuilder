using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThemeParkSimulator.XMLDataObjects;

namespace ThemeParkSimulator
{
    public class Simulator
    {
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
                while (Timer.GetTicks() < 50) ;
            }
            Console.WriteLine("Simulation stoping gracefully");
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
