using System;
using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace Stercroft
{
    public class Unit :GameObject
    {
        private int hp;
        public int movementSpeed{get; protected set;}
        public float attackSpeed{get; protected set;}
        public float attackRange{get; protected set;}

        private Vector2[] movements;

        protected List<Vector2> movementsToMake = new List<Vector2>();

        private float movementSteps = 25;

        private Queue<Vector2> movementOrders = new Queue<Vector2>();

        protected Rectangle body;
        protected Rectangle checkForterrain = new Rectangle(0, 0, 12.5f, 12.5f);

        public Unit()
        {
            movements = new Vector2[]//alla movement directions som programmet ska kolla för varje tile
            {
                new Vector2(1,0),
                new Vector2(0,1),
                new Vector2(-1,0),
                new Vector2(0, -1)
            };
        }

        protected Vector2 movementDirection;//Den movementdirection som karaktären just nu går mot
        private int currentMoveToMake = 1;//vilket move i listan som görs just nu

        private bool directionChanced = true;//om direction changed kommer den att ändra movement direction
        public override void Update()//update metoden fast overrided 
        {
            if(Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))//om man trycker ner left clicks skapa en ny pathfinding request på musens position
            {
                movementsToMake.Clear();
                currentMoveToMake = 1;
                movementDirection.X = 0;//sätter movement direction till 0 så att man slutar gå när den beräknad ny path
                movementDirection.Y = 0;
                directionChanced = true;
                
                Vector2 mousePos = Raylib.GetMousePosition();//skaffar musens position
                FindPath(mousePos);//gör en find path request
                
                checkForterrain.x = mousePos.X - checkForterrain.width/2;//den lilla gröna pluppen som visar vart man klickade
                checkForterrain.y = mousePos.Y - checkForterrain.height/2;
                
            }
            if(movementsToMake.Count > 0 && currentMoveToMake<movementsToMake.Count)// om det finns movement att göra i listan kalla på calculate movement och kör den så att karaktären går
            {   
                CalulateMovement();     
            }
            else//om inget finns se till att listan e helt tom
            {
                
                movementsToMake.Clear();
                currentMoveToMake = 1;
            }

            
            
        }

        private void CalulateMovement()//Kalkylerar hur personen ska gå
        {
            
                
            if(directionChanced)
            {
                
                directionChanced = false;
                movementDirection.X = 0;
                movementDirection.Y = 0;
                
                movementDirection.X = movementsToMake[currentMoveToMake].X - MathF.Round(position.X);//kalkylerar en direction som personen ska gå
                movementDirection.Y = movementsToMake[currentMoveToMake].Y - MathF.Round(position.Y);
                
                if(Math.Abs(movementDirection.X) > Math.Abs(movementDirection.Y))//gör att den directionen skrivs som en 1 eller nolla dvs att om höger så är vectorn (1,0)
                {
                    movementDirection.Y = MathF.Round(movementDirection.Y/MathF.Abs(movementDirection.X));
                    movementDirection.X = MathF.Round(movementDirection.X/MathF.Abs(movementDirection.X));
                    
                }
                else
                {
                    movementDirection.X = MathF.Round(movementDirection.X/MathF.Abs(movementDirection.Y));
                    movementDirection.Y = MathF.Round(movementDirection.Y/MathF.Abs(movementDirection.Y));
                    
                }
                    
                movementDirection.X = MathF.Round(movementDirection.X);
                movementDirection.Y = MathF.Round(movementDirection.Y);

            }
            
            

            position.X += movementDirection.X * movementSpeed * Raylib.GetFrameTime();//förflyttar relativt till movement direction
            position.Y += movementDirection.Y * movementSpeed * Raylib.GetFrameTime();

            //System.Console.WriteLine(position.X);
            //System.Console.WriteLine(position.Y + "y");
            
            if(movementDirection.Y == 0)//gör att när man gått förbi den node som är nästa target så byts det till nästa target och en ny movement kalyleras
            {
                if(movementDirection.X > 0)
                {
                    if(position.X > movementsToMake[currentMoveToMake].X)
                    {
                        directionChanced = true;
                        currentMoveToMake++;
                        
                    }
                }
                else
                {
                    if(position.X < movementsToMake[currentMoveToMake].X)
                    {
                        directionChanced = true;
                        currentMoveToMake++;
                        
                    }
                }
                    
            }
            if(movementDirection.X == 0)//gör samma som den ovan fast för X
            {
                if(movementDirection.Y > 0)
                {
                    if(position.Y > movementsToMake[currentMoveToMake].Y)
                    {
                        directionChanced = true;
                        currentMoveToMake++;
                    }
                }
                else
                {
                    if(position.Y < movementsToMake[currentMoveToMake].Y)
                    {
                        directionChanced = true;
                        currentMoveToMake++;
                        
                    }
                }
                    
            }
            body.x = position.X;//ändrar hitbox till position
            body.y = position.Y;
            
                
 
        }

        private List<Tile> alreadyChecked = new List<Tile>();//alla tiles som redan har kollats
        private Queue<Tile> needToCheck = new Queue<Tile>();//De tiles som måste kollas
        private Tile currentlyChecking;// den tile som just nu kollas
        private void FindPath(Vector2 endPos) //hittar en path från den nuvarande position
        {
            position.X = (float.Parse(Math.Round(position.X/25).ToString())* 25) + 6;//resetar positionen till griden så vi inte uppleverkonstig pathfind
            position.Y = (float.Parse(Math.Round(position.Y/25).ToString())* 25) + 6;
            alreadyChecked.Clear();//gör listorna klara för en ny sökning om något finns i dem
            needToCheck.Clear();
            
            checkForterrain.x = position.X;// sätter hitbox till positionen av spelaren
            checkForterrain.y = position.Y;

            currentlyChecking = new Tile(null, 0, new Vector2(checkForterrain.x,checkForterrain.y));// sätter första tile till current position

            foreach(Vector2 posToCheck in movements)//kollar alla positioner runt current tile
            {
                checkForterrain.x = position.X;
                checkForterrain.y = position.Y;
                checkForterrain.x += posToCheck.X * movementSteps;//sätter hitbox till en positionerna runt som ska kolla
                checkForterrain.y += posToCheck.Y * movementSteps;
                bool didCollide = false;
                foreach(Terrain ter in Terrain.terrains)//kollar om hitbox kolliderar med någon terrain och om det gör det skapa ingen tile och om inte skapa en tile som är möjlig path
                if(Raylib.CheckCollisionRecs(checkForterrain, ter.body))
                {
                    didCollide = true;
                    //System.Console.WriteLine(checkForterrain.x+ " " + checkForterrain.y);
                    
                }
                if(!didCollide)
                {
                    needToCheck.Enqueue(new Tile(currentlyChecking, currentlyChecking.movementCost+ 1, new Vector2(checkForterrain.x,checkForterrain.y)));//skapar en ny tile med current tile som parent och lägger till den i need to check queuen
                }
            }
            alreadyChecked.Add(currentlyChecking);//lägger till currentlychecking i listan av de som redan har checkats
            //System.Console.WriteLine(needToCheck.Count);
            
            bool GoalFound = false;
            while(needToCheck.Count > 0 && !GoalFound)// sålänge de finns tiles att kolla på och målet inte har hittats
            {
                currentlyChecking = needToCheck.Dequeue();// tar en tile från listan och gör det till den som är currently checking
                //System.Console.WriteLine("Checking: " + currentlyChecking.position);
                
                foreach(Vector2 posToCheck in movements)//kollar alla positioner runt current tile
                {
                    checkForterrain.x = currentlyChecking.position.X;
                    checkForterrain.y = currentlyChecking.position.Y;
                    checkForterrain.x += posToCheck.X * movementSteps;//sätter hitbox till en positionerna runt som ska kolla
                    checkForterrain.y += posToCheck.Y * movementSteps;
                    

                    
                    bool didCollide = false;
                    foreach(Terrain ter in Terrain.terrains)//kollar om hitbox kolliderar med någon terrain och om det gör det skapa ingen tile och om inte skapa en tile som är möjlig path
                    if(Raylib.CheckCollisionRecs(checkForterrain, ter.body))
                    {
                        didCollide = true;
                        
                    }

                    if(!didCollide)
                    {
                        Vector2 currentPos = new Vector2(checkForterrain.x,checkForterrain.y);
                        foreach(Tile p in alreadyChecked)if(p.position == currentPos)//om tile redan finns i alreadychecked och movmentkost är mindre än den tile lägg till den i check oxå
                        {
                            if(p.movementCost > currentlyChecking.movementCost+1)
                            {
                                needToCheck.Enqueue(new Tile(currentlyChecking, currentlyChecking.movementCost+ 1, currentPos));
                            }
                        }
                        else
                        {
                            bool doesitExist = false;
                            foreach(Tile c in needToCheck)if(c.position == currentPos)//om tile redan finns i needtocheck så ska den inte finnas flera gånger
                            {
                                doesitExist = true;
                            }
                            if(!doesitExist)
                            {
                                if(!(checkForterrain.x < 0 || checkForterrain.y < 0|| checkForterrain.x > Raylib.GetScreenWidth()||checkForterrain.y > Raylib.GetScreenHeight()))// om den är innanför bounds
                                {
                                    needToCheck.Enqueue(new Tile(currentlyChecking, currentlyChecking.movementCost+ 1, currentPos));
                                }
                            }
                        }
                        
                        
                    }
                    if(checkForterrain.x < endPos.X + 13 && checkForterrain.x > endPos.X - 13 && checkForterrain.y < endPos.Y + 13 && checkForterrain.y > endPos.Y - 13)//kollar om pathfinding är nära målet
                    {
                        //System.Console.WriteLine("GoalFound!!");
                        currentlyChecking = new Tile(currentlyChecking, currentlyChecking.movementCost+ 1, new Vector2(checkForterrain.x , checkForterrain.y));
                        GoalFound = true;
                    }
                    
                    
                }
                alreadyChecked.Add(currentlyChecking);// lägg till den som checkas i redan kollad
                //System.Console.WriteLine(needToCheck.Count);
            }
            //System.Console.WriteLine("starting draw");
            
            Tile previousTile = currentlyChecking;
            List<Vector2> tempList = new List<Vector2>();
            while(previousTile != null)// hämta path
            {
                //System.Console.WriteLine(previousTile.position);
                tempList.Add(previousTile.position);
                previousTile = previousTile.previousTile;
                //System.Console.WriteLine("haj");
            }
            
            for (int i = tempList.Count-1; i >= 0; i--)// reversa listan och lägg till den i listan av movements som ska göra
            {
                movementsToMake.Add(tempList[i]);
            }
            
        }

    }
}
