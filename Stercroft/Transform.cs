using System;
using System.Collections.Generic;
using System.Numerics;

namespace Stercroft
{
    public class Transform<T> where T: struct, IConvertible
    {
        public T X;
        public T Y;

        public Transform(T x, T y)
        {

        }

        public void Translate(Vector2 movementVector)
        {
            //X = movementVector.X;
        }
    }
}
