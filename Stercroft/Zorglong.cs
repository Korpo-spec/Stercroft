
using System;
using System.Collections.Generic;
using Raylib_cs;

namespace Stercroft
{
    public class Zorglong :Unit
    {
        private Rectangle body;
        public Zorglong(float posX, float posY)
        {
            position.X = posX;
            position.Y = posY;
            
            System.Console.WriteLine(position);
            body = new Rectangle(position.X, position.Y, 12.5f, 12.5f);
            
        }
        public override void Draw()
        {
           
            Raylib.DrawRectangleRec(body, Color.BLUE);
            

        }
    }
}
