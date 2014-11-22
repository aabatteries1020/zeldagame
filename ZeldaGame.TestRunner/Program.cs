using NUnit.ConsoleRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaGame.TestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Runner.Main(new string[] { typeof(ZeldaGame.Tests.MovingStateTests).Assembly.Location } );

            Console.ReadKey();
        }
    }
}
