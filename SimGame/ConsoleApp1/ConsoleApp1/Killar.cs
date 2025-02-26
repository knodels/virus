    using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Killar : Virus
    {
        private static Random rand = new Random();
        public Killar(string code, bool reinfection, float infection, float letality) : base(code, reinfection, infection, letality)
        {
            _letality = letality+(float)rand.Next(-10, 10) / 100;
        }
        public override int AgeToInfect => 12;
        public override int DayToRecover => 7;
        public override bool Death(Person person)
        {
            if(rand.NextDouble() <= Lethality)
            {
               person.Death();
               return true;
            }
            return false;
        }

        public override void Infect(Person person)
        {
            if (person.Immunity <= Infection)
            {
                person.Infect();
            }
        }
    }
}
