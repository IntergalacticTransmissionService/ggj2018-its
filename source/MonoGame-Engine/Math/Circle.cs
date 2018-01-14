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
    }
}
