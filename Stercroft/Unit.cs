using System;
using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace Stercroft
{
    public class Unit :GameObject
    {
        private int hp;
        public int movementSpeed{get; set;}
        public float attackSpeed{get; private set;}
        public float attackRange{get; private set;}

        private Vector2[] movements;

        protected List<Vector2> movementsToMake = new List<Vector2>();

        private float movementSteps = 25;

        private Queue<Vector2> movementOrders = new Queue<Vector2>();

        private Rectangle body;
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
                
                Vector2 mousePos = Raylib.GetMousePosition();
                FindPath(mousePos);
                System.Console.WriteLine(mousePos);
                checkForterrain.x = mousePos.X;
                checkForterrain.y = mousePos.Y;
                foreach(Terrain ter in Terrain.terrains)
                if(Raylib.CheckCollisionRecs(checkForterrain, ter.body))
                {
                    System.Console.WriteLine("waho");
                }
            }
            if(movementsToMake.Count > 0)
            {
                
                if(directionChanced)
                {
                    directionChanced = false;
                    movementDirection.X = 0;
                    movementDirection.Y = 0;
                    movementDirection.X = position.X - movementsToMake[currentMoveToMake].X;
                    movementDirection.Y = position.Y - movementsToMake[currentMoveToMake].Y;
                    if(Math.Abs(movementDirection.X) > Math.Abs(movementDirection.Y))
                    {
                        movementDirection.X = movementDirection.X/movementDirection.X;
                        movementDirection.Y = movementDirection.Y/movementDirection.X;
                    }
                    else
                    {
                        movementDirection.X = movementDirection.X/movementDirection.Y;
                        movementDirection.Y = movementDirection.Y/movementDirection.Y;
                    }
                    
                    movementDirection.X = (float)Math.Round(movementDirection.X);
                    movementDirection.Y = (float)Math.Round(movementDirection.Y);
                   
                    

                }
                
                

                position.X += movementDirection.X * movementSpeed * Raylib.GetFrameTime();
                position.Y += movementDirection.Y * movementSpeed * Raylib.GetFrameTime();
                
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
                    System.Console.WriteLine(checkForterrain.x+ " " + checkForterrain.y);
                    
                }
                if(!didCollide)
                {
                    needToCheck.Enqueue(new Tile(currentlyChecking, currentlyChecking.movementCost+ 1, new Vector2(checkForterrain.x,checkForterrain.y)));
                }
            }
            alreadyChecked.Add(currentlyChecking);
            System.Console.WriteLine(needToCheck.Count);
            
            bool GoalFound = false;
            while(needToCheck.Count > 0 && !GoalFound)
            {
                currentlyChecking = needToCheck.Dequeue();
                System.Console.WriteLine("Checking: " + currentlyChecking.position);
                
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
                        System.Console.WriteLine("GoalFound!!");
                        GoalFound = true;
                    }
                    
                    
                }
                alreadyChecked.Add(currentlyChecking);
                System.Console.WriteLine(needToCheck.Count);
            }
            System.Console.WriteLine("starting draw");
            Raylib.BeginMode2D(GameObject.camera);
            Tile previousTile = currentlyChecking;
            while(previousTile != null)
            {
                System.Console.WriteLine(previousTile.position);
                movementsToMake.Add(previousTile.position);
                previousTile = previousTile.previousTile;
                System.Console.WriteLine("haj");
            }
            Raylib.EndMode2D();

            
        }




        

    }
}
