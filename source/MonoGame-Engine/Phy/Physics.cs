using Microsoft.Xna.Framework;
using MonoGame_Engine.Math;

namespace MonoGame_Engine.Phy
{
    public class Physics
    {
        public Vector2 Pos;
        public float Rot;

        public Vector2 Spd;
        public float RotSpd;

        public Vector2 Accel;
        public float Dmp = 1.0f;

        public readonly Circle HitBox;

        public Physics(float? radius)
        {
            Pos = new Vector2();
            Rot = 0;

            Spd = new Vector2();
            RotSpd = 0;

            HitBox = radius.HasValue ? new Circle(Vector2.Zero, radius.Value) : null;
        }

        public virtual void Update(GameTime gameTime)
        {
            // timestep
            var delta = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            // update movement
            Spd.X += Accel.X * delta;
            Spd.Y += Accel.Y * delta;
            Pos.X = Pos.X + Spd.X * delta;
            Pos.Y = Pos.Y + Spd.Y * delta;
            Rot = MathHelper.Clamp(Rot + RotSpd * delta, -MathHelper.Pi, MathHelper.Pi);

            Spd.X *= Dmp;
            Spd.Y *= Dmp;

            Accel = Vector2.Zero;

            // update hitbox
            if (HitBox != null)
            {
                HitBox.Center.X = Pos.X;
                HitBox.Center.Y = Pos.Y;
            }
        }

        public bool CollidesWith(Physics other)
        {
            if (HitBox == null || other.HitBox == null)
                return false;

            return HitBox.Intersects(other.HitBox);
        }

        public bool CollidesWith(Vector2 pos)
        {
            if (HitBox == null || pos == null)
                return false;

            return HitBox.Contains(pos);
        }

    }
}
