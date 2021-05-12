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
            movements = new Vector2[]
            {
                new Vector2(1,0),
                new Vector2(0,1),
                new Vector2(-1,0),
                new Vector2(0, -1)
            };
        }

        protected Vector2 movementDirection;
        private int currentMoveToMake = 1;

        private bool directionChanced = true;
        public override void Update()
        {
            if(Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
            {
                movementsToMake.Clear();
                currentMoveToMake = 1;
                movementDirection.X = 0;
                movementDirection.Y = 0;
                directionChanced = true;
                
                Vector2 mousePos = Raylib.GetMousePosition();
                FindPath(mousePos);
                
                checkForterrain.x = mousePos.X - checkForterrain.width/2;
                checkForterrain.y = mousePos.Y - checkForterrain.height/2;
                
            }
            if(movementsToMake.Count > 0 && currentMoveToMake<movementsToMake.Count)
            {   
                CalulateMovement();     
            }
            else
            {
                
                movementsToMake.Clear();
                currentMoveToMake = 1;
            }

            
            
        }

        private void CalulateMovement()
        {
            
                
            if(directionChanced)
            {
                
                directionChanced = false;
                movementDirection.X = 0;
                movementDirection.Y = 0;
                
                movementDirection.X = movementsToMake[currentMoveToMake].X - MathF.Round(position.X);
                movementDirection.Y = movementsToMake[currentMoveToMake].Y - MathF.Round(position.Y);
                
                if(Math.Abs(movementDirection.X) > Math.Abs(movementDirection.Y))
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
            
            

            position.X += movementDirection.X * movementSpeed * Raylib.GetFrameTime();
            position.Y += movementDirection.Y * movementSpeed * Raylib.GetFrameTime();

            //System.Console.WriteLine(position.X);
            //System.Console.WriteLine(position.Y + "y");
            
            if(movementDirection.Y == 0)
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
            if(movementDirection.X == 0)
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
            body.x = position.X;
            body.y = position.Y;
                
 
        }

        private List<Tile> alreadyChecked = new List<Tile>();
        private Queue<Tile> needToCheck = new Queue<Tile>();
        private Tile currentlyChecking;
        private void FindPath(Vector2 endPos) 
        {
            position.X = (float.Parse(Math.Round(position.X/25).ToString())* 25) + 6;
            position.Y = (float.Parse(Math.Round(position.Y/25).ToString())* 25) + 6;
            alreadyChecked.Clear();
            needToCheck.Clear();
            
            checkForterrain.x = position.X;
            checkForterrain.y = position.Y;

            currentlyChecking = new Tile(null, 0, new Vector2(checkForterrain.x,checkForterrain.y));

            foreach(Vector2 posToCheck in movements)
            {
                checkForterrain.x = position.X;
                checkForterrain.y = position.Y;
                checkForterrain.x += posToCheck.X * movementSteps;
                checkForterrain.y += posToCheck.Y * movementSteps;
                bool didCollide = false;
                foreach(Terrain ter in Terrain.terrains)
                if(Raylib.CheckCollisionRecs(checkForterrain, ter.body))
                {
                    didCollide = true;
                    //System.Console.WriteLine(checkForterrain.x+ " " + checkForterrain.y);
                    
                }
                if(!didCollide)
                {
                    needToCheck.Enqueue(new Tile(currentlyChecking, currentlyChecking.movementCost+ 1, new Vector2(checkForterrain.x,checkForterrain.y)));
                }
            }
            alreadyChecked.Add(currentlyChecking);
            //System.Console.WriteLine(needToCheck.Count);
            
            bool GoalFound = false;
            while(needToCheck.Count > 0 && !GoalFound)
            {
                currentlyChecking = needToCheck.Dequeue();
                //System.Console.WriteLine("Checking: " + currentlyChecking.position);
                
                foreach(Vector2 posToCheck in movements)
                {
                    checkForterrain.x = currentlyChecking.position.X;
                    checkForterrain.y = currentlyChecking.position.Y;
                    checkForterrain.x += posToCheck.X * movementSteps;
                    checkForterrain.y += posToCheck.Y * movementSteps;
                    

                    
                    bool didCollide = false;
                    foreach(Terrain ter in Terrain.terrains)
                    if(Raylib.CheckCollisionRecs(checkForterrain, ter.body))
                    {
                        didCollide = true;
                        
                    }

                    if(!didCollide)
                    {
                        Vector2 currentPos = new Vector2(checkForterrain.x,checkForterrain.y);
                        foreach(Tile p in alreadyChecked)if(p.position == currentPos)
                        {
                            if(p.movementCost > currentlyChecking.movementCost+1)
                            {
                                needToCheck.Enqueue(new Tile(currentlyChecking, currentlyChecking.movementCost+ 1, currentPos));
                            }
                        }
                        else
                        {
                            bool doesitExist = false;
                            foreach(Tile c in needToCheck)if(c.position == currentPos)
                            {
                                doesitExist = true;
                            }
                            if(!doesitExist)
                            {
                                if(!(checkForterrain.x < 0 || checkForterrain.y < 0|| checkForterrain.x > Raylib.GetScreenWidth()||checkForterrain.y > Raylib.GetScreenHeight()))
                                {
                                    needToCheck.Enqueue(new Tile(currentlyChecking, currentlyChecking.movementCost+ 1, currentPos));
                                }
                            }
                        }
                        
                        
                    }
                    if(checkForterrain.x < endPos.X + 13 && checkForterrain.x > endPos.X - 13 && checkForterrain.y < endPos.Y + 13 && checkForterrain.y > endPos.Y - 13)
                    {
                        //System.Console.WriteLine("GoalFound!!");
                        currentlyChecking = new Tile(currentlyChecking, currentlyChecking.movementCost+ 1, new Vector2(checkForterrain.x , checkForterrain.y));
                        GoalFound = true;
                    }
                    
                    
                }
                alreadyChecked.Add(currentlyChecking);
                //System.Console.WriteLine(needToCheck.Count);
            }
            //System.Console.WriteLine("starting draw");
            
            Tile previousTile = currentlyChecking;
            List<Vector2> tempList = new List<Vector2>();
            while(previousTile != null)
            {
                //System.Console.WriteLine(previousTile.position);
                tempList.Add(previousTile.position);
                previousTile = previousTile.previousTile;
                //System.Console.WriteLine("haj");
            }
            
            for (int i = tempList.Count-1; i >= 0; i--)
            {
                movementsToMake.Add(tempList[i]);
            }
            
        }

    }
}
