using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Security.Cryptography;

namespace ConsoleApplication1
{
    class Checker
    {
        public static int totalAllThreads = 0;
        private static bool tel;
        private static Object LOCK = new Object();

        public Checker(int lower, int upper, int m, int p, int u)
        {
            if (u == 0)
                tel = true;
            else
            {
                tel = false;
                totalAllThreads = 1;
            }

            Thread[] ts = new Thread[p];
            for(int i = 0; i < p; i++)
            {
                int from = lower + (int)((i * (long)(upper-lower)) / p);
                int to = lower + (int)(((i +1)* (long)(upper-lower)) / p);
                if(tel)
                    ts[i] = new Thread(() => CheckBoundariesTel(from, to, m));
                else ts[i] = new Thread(() => CheckBoundariesList(from, to, m));
            }
            
            for(int i = 0; i<p; i++)
            {
                ts[i].Start();
            }

            for (int i = 0; i < p; i++)
            {
                ts[i].Join();
            }
            if(tel)
                Console.WriteLine(totalAllThreads);
            
        }

        private static int sumNumber(int number)
        {
            int sum = 0;
            int tenPower = 1;
            for (int k = 1; k < 10; k++)
            {
                if (k != 1)
                    tenPower *= 10;
                sum += ((number % (tenPower * 10)) / tenPower) * k;
            }
            return sum;
        }

        private static void CheckBoundariesList(int low, int upper, int m)
        {
            for (int i = low; i < upper; i++)
            {
                int sum = sumNumber(i);
                if (sum % m == 0)
                {
                    lock (LOCK)
                    {
                        Console.WriteLine(totalAllThreads + " " + i);
                        Interlocked.Increment(ref totalAllThreads);
                    }
                }
            }
        }

        private static void CheckBoundariesTel(int low, int upper, int m)
        {
            int total = 0;
            for (int i = low; i < upper; i++)
            {
                int sum = sumNumber(i);
                if (sum % m == 0)
                    total++;
            }
            Interlocked.Add(ref totalAllThreads, total);
        }
    }
}
