using System;
using Microsoft.Xna.Framework;
using static System.Math;

namespace MonoGame_Engine.Math
{
    public class Circle
    {
        public Vector2 Center;
        public float Radius;

        public Circle(Vector2 center, float radius)
        {
            Center.X = center.X;
            Center.Y = center.Y;
            Radius = radius;
        }

        public bool Intersects(Circle other)
        {
            var dx = other.Center.X - Center.X;
            var dy = other.Center.Y - Center.Y;
            var dist = Sqrt(dx * dx + dy * dy);
            return dist < (Radius + other.Radius);
        }

        internal bool Contains(Vector2 pos)
        {
            var dist = Vector2.Distance(Center, pos);
            return dist < Radius;
        }

        public static Circle operator +(Circle c, Vector2 v) => new Circle(c.Center + v, c.Radius);
        public static Circle operator +(Vector2 v,Circle c) => new Circle(c.Center + v, c.Radius);
    }
}
