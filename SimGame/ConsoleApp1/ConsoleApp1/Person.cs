using ConsoleApp1.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Person
    {
      
        private string _gender;
        private float _immunity;
        private int _age;
        private float _initialImmunity;
        private bool _totalImmunity;
        private const float _coefLostImmunity = 0.000017f;
        private int _friends;
        private bool _status;
        private int _infectionDays;
        private bool totalImunity;
        private bool _isAlive;
        public int Age => _age;

        public string Gender => _gender;
        public bool isAlive => _isAlive;
        public float Immunity => _immunity;
        public bool Status => _status;   
        public bool TotalImmunity => _totalImmunity;
        public int MaxAge = 80;
        public int Friends => _friends;
        
        public Person(string Gender, int Age, float Immunity)
        {
            _gender = Gender;
            _age = Age;
            _initialImmunity = Immunity;
            _totalImmunity = false;
            _isAlive = true;
            _friends = (int)Gousian.RandNormal(3, 1);
            _infectionDays = 0;

            _status = false;
            UpdateImmunity();
        }      
        public void UpdateAge()
        {
            _age++;
            UpdateImmunity();
        }
        public int UpdateInfect()
        {
            if (!Status) _infectionDays = 0;
            else _infectionDays++;
            return _infectionDays;
        }
        public void Death()=>_isAlive = false;
        public void Infect() => _status = true;
        public void Recover() => _status = false;
        public void CreateTotalImunity() => totalImunity=true;
        private void UpdateImmunity()
        {
            if (Age >= 29200)
                return;
            _immunity = _initialImmunity - _coefLostImmunity * Age;
        }
    }
}
