using System;

namespace MyFirstGeneticAlgorythm
{
    class Program
    {
        static void Main(string[] args)
        {
            TestShakespeare test = new TestShakespeare();
            test.Start();
            test.Update();
            Console.ReadLine();
        }
    }
}
