using System;
using Raylib_cs;
using System.Numerics;


namespace Stercroft
{
    public class Animation
    {
        private float framerate; //Hur snabbt animationen spelas
        private int framesize;//Hur stor varje ruta är
        private Rectangle rect;//Den delen av hela spritesheet som ska ritas
        private float time;//En variabel för att hålla koll på tiden
        private int currentFrame;//Den framen som visas just nu

        private Texture2D texture;//Hela bilden
        public Animation(Texture2D tex, int framesizze)//konstruktor for animation klassen
        {
            texture = tex;//sätter texture
            texture.height = (int)(texture.height * 0.5f);//här ändras formatet till något som fungerar bättre just nu
            texture.width = (int)(texture.width * 0.5f);
            framesize = (int)(framesizze * 0.5f);
            framerate = 15;//sätter framerate, detta bör göras som parameter i konstruktor
            rect = new Rectangle(0,0 , framesize, framesize);//skapar rectangeln som kommer användas för att kolla vilken frame som ska visas
            
        }
        
        public void DrawFrame(Vector2 animDirection, Vector2 position)//Ritar ut den nuvarande framen på den position och i den riktning karaktären rör sig
        {
            time += Raylib.GetFrameTime();//updaterar tiden 
            
            
            if(time > 1/framerate)//om tiden har gått så att det är dags att byta frame gör det
            {
                time = 0;//reset time
                currentFrame++;//ökar currentframe så nästa visas
                rect.x += framesize;//ökar rectangelns position så att nästa bild visas
                if(rect.x == texture.width)//om vi når slutet på spritesheet så börjar den om
                {
                    rect.x = 0;
                    currentFrame = 0;
                }

                if(animDirection.X == -1)//Kollar vilken direction man går i och ändrar sprite raden till den verisionen
                {
                    rect.y = framesize*1;
                }
                else if(animDirection.X == 1)
                {
                    rect.y = framesize*3;
                }
                
                if(animDirection.Y == -1)
                {
                    rect.y = 0;
                }
                else if(animDirection.Y == 1)
                {
                    rect.y =framesize*2;
                }
                

            }
            
            position.X -= framesize/3;//korrigerar platsen som karaktären visas på
            position.Y -= framesize/2;

            Raylib.DrawTextureRec(texture,rect,position, Color.WHITE);
        }
    }
}
