using System;
using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;

namespace Stercroft
{
    class Program
    {
        static void Main(string[] args)
        {

            Dictionary<string, int> stats = new Dictionary<string, int>();

            stats.Add("Strength", 5 );
            stats.Add("Charisma", 7);
            stats.Add("Dexerity", 15);
            stats.Add("Height", 180);
            stats.Add("HP regen", 1);

            Console.WriteLine("Create Custom stat");
            Console.Write("Write name of the stat: ");
            string name = Console.ReadLine();
            bool acceptableAns = false;
            int output = 0;
            while (!acceptableAns)
            {
                Console.WriteLine("Write a valid integer: ");
                string userInput = Console.ReadLine();
                if(int.TryParse(userInput, out output))
                {
                    acceptableAns = true;
                }
            }
            stats.Add(name, output);
            foreach(string key in stats.Keys)
            {
                Console.WriteLine(key + ": " + stats[key]);
            }

            


            Console.ReadLine();
            Raylib.InitWindow(1000, 800, "Stercroft"); //Sets the size and framerate of the game window
            Raylib.SetTargetFPS(60);
            Console.WriteLine("Hello World!");
            new Terrain(50, 50);
            new Terrain(25, 75);
            new Terrain(25, 100);
            Zorglong zorglis = new Zorglong(81f , 56f);
            
            //Zorglong.movementSpeed = 65;
            
            
            GameObject.camera.zoom = 1f; //A zoom variable for the camera to work
            GameObject.camera.target = new Vector2(0,0); // Where the camera is pointed at
            GameObject.camera.offset = new Vector2(0, 0);
            

            while(!Raylib.WindowShouldClose())
            {
                GameObject.UpdateAll();
                GameObject.DrawAll();
            }
        }
    }
}
