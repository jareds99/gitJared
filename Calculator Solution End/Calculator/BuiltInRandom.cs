using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class BuiltInRandom : IRandom
    {
        private Random _random = new Random();

        public int GetRandomNumber()
        {
            return this._random.Next(100) + 1;
        }
    }
}
