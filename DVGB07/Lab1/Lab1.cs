using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DVGB07.Lab1
{
    internal class Lab1
    {
        /// <summary>
        /// Laboration 1 består av en uppsättning mindre övningsuppgifter, som avslutas med en lite mer komplicerad uppgift.
        /// </summary>
        public Lab1()
        {
            md();
        }
        /// <summary>
        /// Skriv ett konsollprogram som skriver jämna heltal från 0 till 30. 
        /// </summary>
        void EvenNum()
        {
            int num = 0;
            while (num <= 30)
            {
                if (num % 2 == 0)
                    Console.WriteLine(num);
                num++;
            }
            Console.WriteLine("DONE!");
        }
        /// <summary>
        /// 2. Skriv ett konsollprogram som läser in ett tal från användaren, och skriver ut om talet är positivt, negativt eller lika med 0. 
        /// </summary>
        void CheckNum()
        {
            string str_num = Console.ReadLine();
            int num = int.Parse(str_num);
            if (num == 0)
                Console.WriteLine("The number is zero");
            else if (num > 0)
                Console.WriteLine("The number is positive");
            else 
                Console.WriteLine("The number is negative");
        }
        /// <summary>
        /// 3. Skriv ett konsollprogram som läser in ett tal n, och sedan läser in n värden från användaren. Därefter ska programmet skriva ut det minsta och det största värdet. 
        /// </summary>
        void MinMax()
        {
            Console.WriteLine("enter the size of array:");
            int n = int.Parse((string) Console.ReadLine());

            int min=0, max=0;
            bool isFirst = true;

            while(n != 0)
            {
                int num= int.Parse(Console.ReadLine());                

                if (isFirst)
                {
                    min = num;
                    max = num;
                    isFirst = false;
                }
                else
                {
                    if( num < min ) 
                    {
                        min = num;
                    }
                    else if( num > max )
                    {
                        max= num;
                    }

                }

                n--;
            }
            Console.WriteLine($"min= {min}, max= {max}");
        }

        /// <summary>
        /// 4. Skriv ett konsollprogram som läser in en text och skriver ut hur många gånger ordföljden AB finns i texten. 
        /// </summary>
        void ordFoljd()
        {
            string text = Console.ReadLine();
            int len = text.Length;
            Console.WriteLine($"len= {len}");
           
            int count =0;
            for (int i = 0; i < len-1; i++)
            {
                //Console.WriteLine($"text[{i}] = {text[i]}");

                if (text[i] == 'A' && text[i + 1] == 'B')
                {
                    count++;
                }
            }
            Console.WriteLine($"number of 'AB'= {count}");
        }

        /// <summary>
        /// 5. Skriv ett konsollprogram som använder en vektor (array) för att ta emot 10 decimaltal, och sedan skriva ut medianen och medelvärdet på talen. 
        /// </summary>
        void md()
        {
            int n =10;
            Console.WriteLine("Skriv in 10 decimal tal:");

            decimal[] array= new decimal[n];
            decimal avg=0, mdn=0;

            for (int i=0; i<n; i++)
            {
                array[i]= decimal.Parse(Console.ReadLine());
            }
            for (int i = 0; i < n; i++)
            {
                avg += array[i];                
            }
            avg /= n;

            for (int i =0; i < n; i++)
            {
                for (int j= 0; j < n-1-i; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        decimal temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
                
            }            
                if (n % 2 == 0)
                {
                    //mdn = (array[n / 2] + array[(n / 2) + 1]) / 2;
                int m1 = n / 2;
                int m2 = n / 2 - 1;
                mdn= (array[m1]+ array[m2])/2;
                }
                else
                {
                mdn = (array[(n / 2)]);
                }



            Console.WriteLine($"Avrage= {avg}, Median= {mdn}");
            Console.WriteLine();


        }
    }
}
