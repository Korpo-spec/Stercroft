
using System;
using System.Collections.Generic;
using Raylib_cs;
using System.Numerics;

namespace Stercroft
{
    public class Zorglong :Unit
    {
        Animation anim;
        public Zorglong(float posX, float posY)
        {
            position.X = posX;
            position.Y = posY;
            anim = new Animation(Raylib.LoadTexture("professor_walk_cycle_no_hat.png"), 64);//Ger zorglong sin speciella Anim klass
            
            movementSpeed = 65;// hur snabbt man rör sig
            body = new Rectangle(position.X, position.Y, 12.5f, 12.5f);//hitbox för karaktären
            
            
        }
        
        
        public override void Draw()
        {
           
            
            Raylib.DrawRectangleRec(checkForterrain, Color.GREEN);

            foreach(Vector2 move in movementsToMake)
            {
                Raylib.DrawRectangle((int)move.X, (int)move.Y , 12, 12, Color.YELLOW);
            }
            
            //Raylib.DrawRectangleRec(body, Color.BLUE);
            anim.DrawFrame(movementDirection, position);

        }
    }
}
