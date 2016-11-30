using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = Console.ReadLine();
            string[] numbers = s.Split(' ');            
            int l = int.Parse(numbers[0]);
            int b = int.Parse(numbers[1]);
            int e = int.Parse(numbers[2]);
            int m = int.Parse(numbers[3]);
            int p = int.Parse(numbers[4]);
            int u = int.Parse(numbers[5]);
            string hash = numbers[6];
            Checker c = new Checker(b, e, m, p, u, hash);
            Console.ReadLine();
        }
    }
}
