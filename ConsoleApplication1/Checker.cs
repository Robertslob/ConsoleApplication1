﻿using System;
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
        public static Locks locker;

        public Checker(int l, int lower, int upper, int m, int p, int u, string s)
        {
            if (l == 1)
                locker = new CSharpLock();
            else locker = new OwnLock();

            if (u == 1)
                totalAllThreads = 1;

            Thread[] ts = new Thread[p];
            for(int i = 0; i < p; i++)
            {
                int from = lower + (int)((i * (long)(upper-lower)) / p);
                int to = lower + (int)(((i +1)* (long)(upper-lower)) / p);
                switch (u)
                {
                    case 0:
                        ts[i] = new Thread(() => CheckBoundariesTel(from, to, m));
                        break;
                    case 1:
                        ts[i] = new Thread(() => CheckBoundariesList(from, to, m));
                        break;
                    case 2:
                        ts[i] = new Thread(() => CheckBoundariesHash(from, to, m, s));
                        break;
                }
            }
            
            for(int i = 0; i<p; i++)
            {
                ts[i].Start();
            }

            for (int i = 0; i < p; i++)
            {
                ts[i].Join();
            }
            if(u ==0)
                Console.WriteLine(totalAllThreads);
            // If no matching hash has been found
            if (totalAllThreads == 0 && u == 2)
                Console.WriteLine("-1");
            
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
                    locker.LockList(i);
            }
        }

        public static void ListWriteIncr(int i)
        {
            Console.WriteLine(totalAllThreads + " " + i);
            totalAllThreads++;
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
            locker.LockTel(total);
        }

        public static void TelAdd(int i)
        {
            totalAllThreads += i;
        }

        private static void CheckBoundariesHash(int low, int upper, int m, string hash)
        {
            SHA1Managed sha = new SHA1Managed();
            for (int i = low; i < upper; i++)
            {
                if (totalAllThreads == 0)
                {
                    int sum = sumNumber(i);
                    if (sum % m == 0)
                    {
                        // Credits for converting the int to a hash in the correct form go to Mitch
                        // http://stackoverflow.com/questions/17292366/hashing-with-sha1-algorithm-in-c-sharp
                        byte[] shaHash = sha.ComputeHash(Encoding.UTF8.GetBytes(i.ToString()));
                        if (string.Join("", shaHash.Select(b => b.ToString("x2")).ToArray()) == hash)
                        {
                            locker.LockHash(i);
                        }
                    }                        
                }
                // Else some other thread has found a matching hash, thus we terminate
                else break;
            }
        }

        public static void HashWriteIncr(int i)
        {
            Console.WriteLine(i);
            totalAllThreads++;
        }
    }
}
