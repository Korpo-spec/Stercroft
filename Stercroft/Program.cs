using System;

namespace Stercroft
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            while(true)
            {
                GameObject.UpdateAll();
                GameObject.DrawAll();
            }
        }
    }
}
