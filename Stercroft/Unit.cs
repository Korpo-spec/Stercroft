using System;
using Raylib_cs;

namespace Stercroft
{
    public class Unit :GameObject
    {
        private int hp;
        public int movementSpeed{get; private set;}
        public float attackSpeed{get; private set;}
        public float attackRange{get; private set;}

        private Rectangle body;

        

    }
}
