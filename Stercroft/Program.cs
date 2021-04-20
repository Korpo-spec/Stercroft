using System;
using System.Numerics;
using Raylib_cs;

namespace Stercroft
{
    class Program
    {
        static void Main(string[] args)
        {
            Raylib.InitWindow(1000, 800, "Stercroft"); //Sets the size and framerate of the game window
            Raylib.SetTargetFPS(60);
            Console.WriteLine("Hello World!");
            new Terrain(50, 50);
            new Terrain(25, 75);
            new Zorglong(31.25f , 56.25f);
            GameObject.camera.zoom = 1f; //A zoom variable for the camera to work
            GameObject.camera.target = new Vector2(0,0); // Where the camera is pointed at
            GameObject.camera.offset = new Vector2(500, 400);

            while(!Raylib.WindowShouldClose())
            {
                GameObject.UpdateAll();
                GameObject.DrawAll();
            }
        }
    }
}
