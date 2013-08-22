using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager m = new Manager();
            m.Run();
            Console.ReadLine();
        }
    }
}
