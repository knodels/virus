using ConsoleApp1.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Observer
    {
        private Simulator _simulator;

        public event Action<Simulator> OnEndSimulation;
        public Observer(ref Simulator simulator)
        {
            _simulator = simulator;
        }

        public void Start()
        {
            var SimThread = new Thread(_simulator.RunSimmulation);
            SimThread.Start();

            var ExitThread = new Thread(() => EarlyExit(SimThread));
            ExitThread.Start();

            SimThread.Join();
            OnEndSimulation?.Invoke(_simulator);
        }

        private void EarlyExit(Thread t)
        {
            while (t.IsAlive)
            {
                if (Console.ReadKey().Key == ConsoleKey.Enter && t.IsAlive)
                {
                    t.Abort();
                }
            }
        }
    }
}
