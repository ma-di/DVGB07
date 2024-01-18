using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVGB07.Lab1
{
    internal class EvenNum
    {
        public EvenNum()
        {
            int tal = 0;
            while (tal <= 30)
            {
                if (tal % 2 == 0)
                    Console.WriteLine(tal);
                tal++;
            }
            Console.WriteLine("DONE!");
        }

    }
}
