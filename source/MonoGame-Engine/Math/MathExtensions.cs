using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MonoGame_Engine.Math
{
    public static class MathExtensions
    {
        public static Rectangle FromVectors(params Vector2[] vectors)
        {
            float minX = vectors.Select(v => v.X).Min();
            float maxX = vectors.Select(v => v.X).Max();
            float minY = vectors.Select(v => v.Y).Min();
            float maxY = vectors.Select(v => v.Y).Max();

            return new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
        }
    }
}
