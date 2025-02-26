using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1.Utils
{
    public class Simulator
    {
        private const double _mortality = (double)14 / 1000;
        private const double _birthrate = (double)8 / 1000;

        private static Random random = new Random();
        
        private Virus _virus;
        private List<Person> _alive;
        private List<Person> _dead;
        private int _maxDays;
        private int _days;
        private int _faling;
        private int _recovered;
        public int Faling => _faling;
        public int TotalPopulation => _alive.Count;
        
        public int DeadPopulation => _dead.Count;
        public int Recovered=>_recovered;
        public int Days => _days;                                                                                                                                                                           
        public Simulator(int countPopulation, int maxDays,Virus virus)
        {
            _alive = new List<Person>();
            _dead = new List<Person>();
            _days = 1;
            _faling = 0;
            _recovered = 0;
            _maxDays = maxDays;          
            _virus = virus;
            Population(countPopulation);
            
        }
           

        private void Mortality()
        {
            int mort = (int)Math.Round(_mortality * _alive.Count / 365);
            List<Person> toDead = _alive.GetRange(0, mort);
            _alive.RemoveRange(0, mort);
            _dead.AddRange(toDead);
        }
        public void RunSimmulation()
        {
            StartInfection();
            for (int i = 1; i <= _maxDays; i++)
            {
                _days = i;
                _alive.RemoveAll((p) =>
                {
                    if (!p.isAlive)
                    {
                        _dead.Add(p);
                        return true;
                    }
                    return false;
                });
                if (i % 365 == 0)
                    _alive.RemoveAll((p) =>
                    {
                        p.UpdateAge();
                        if (p.Age >= p.MaxAge)
                        {
                            _dead.Add(p);
                            return true;
                        }
                        return false;
                    });

                Infection();
                Mortality();
                Birth();
            }
        }
       
        private void Birth()
        {
            int birth = (int)Math.Round(_birthrate * _alive.Count / 365);
            for (int i = 0; i < birth; i++)
            {
                Person newPerson = new Person(
                    random.Next(0, 2) == 0 ? "Ж" : "М", 0,
                    (float)random.Next(65, 76) / 100);
                _alive.Add(newPerson);
            }
        }
        private void Population(int countPopulation)
        {
            for (int i = 0; i < countPopulation; i++)
            {
                Person newPerson = new Person(
                    random.Next(0, 2) == 0 ? "Ж" : "М",
                    random.Next(0, 81),
                    (float)random.Next(65, 76) / 100
                    );
                if (newPerson.Age >= newPerson.MaxAge)
                    _dead.Add(newPerson);
                else
                    _alive.Add(newPerson);
            }

        }
        private void StartInfection()
        {
            for (int i = 0; i < Math.Round(_alive.Count * 0.02); i++)
            {
                _alive.Find((p) => (p.Age >= _virus.AgeToInfect) && (!p.Status)).Infect();
                _faling++;
            }
            _alive = _alive.OrderBy(_ => random.Next()).ToList();
        }

        public int InfectionPopulation() => _alive.FindAll((p) => (p.Status)).Count;

        private void Infection()
        {
           var allInfected = _alive.FindAll((p) => p.Status);
           foreach(Person p in allInfected)
           {
                if (_virus.Death(p)) continue;
                if (p.UpdateInfect() == _virus.DayToRecover)
                {
                    if (!_virus.Reinfection)
                        p.CreateTotalImunity();
                    p.Recover();
                        _recovered++;
                        continue;
                }
                if (random.Next(101) <= 28) continue;
                for(int i= 0; i < p.Friends/2; i++) { 
                    Person meeting = _alive[random.Next(0, _alive.Count)];
                    if (!meeting.Status && meeting.Age >= _virus.AgeToInfect && !meeting.TotalImmunity)
                    {
                        _virus.Infect(meeting);
                        _faling++;
                    }
                }
           }

        }
    }
}
