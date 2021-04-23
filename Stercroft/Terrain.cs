using System.Collections.Generic;
using System;
using System.Collections.Generic;
using Raylib_cs;

namespace Stercroft
{
    public class Terrain :GameObject
    {
        public static List<Terrain> terrains = new List<Terrain>();

        public Rectangle body{get;private set;}

        public Terrain(float posX, float posY)
        {
            position.X = posX;
            position.Y = posY;
            terrains.Add(this);
            System.Console.WriteLine(position);
            body = new Rectangle(position.X, position.Y, 25, 25);
        }

        public override void Update()
        {
           
        }

        public override void Draw()
        {
           
            Raylib.DrawRectangleRec(body, Color.RED);
            

        }




    }
}
