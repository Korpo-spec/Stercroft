using System;
using System.Numerics;

namespace Stercroft
{
    public class Tile
    {

        public Vector2 position;
        public Tile previousTile;
        public int movementCost;

        public Tile(Tile previousTilee, int movementCost, Vector2 position)
        {
            this.previousTile = previousTilee;
            this.movementCost = movementCost;
            this.position = position;
        }
    }
}
