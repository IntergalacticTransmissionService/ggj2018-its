using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        private Circle[] HitBox
        {
            get
            {
                var matrix = Matrix.CreateRotationZ(this.Rot);
                return UntraslatedHitBox.Select(box =>
                {
                    return new Circle(Vector2.Transform(box.Center, matrix), box.Radius) + this.Pos;
                }).ToArray();
            }
        }
        private readonly Circle[] UntraslatedHitBox;
        public float? Radius { get; }

        public Physics(float? radius) : this(GetData(radius)) { }

        private static (float radius, Vector2 relativePositionToCenter)[] GetData(float? radius)
        {
            if (radius.HasValue)
                return new[] { (radius.Value, Vector2.Zero) };
            return new(float radius, Vector2 relativePositionToCenter)[0];
        }

        public Physics(params (float radius, Vector2 relativePositionToCenter)[] data)
        {
            Pos = new Vector2();
            Rot = 0;

            Spd = new Vector2();
            RotSpd = 0;

            UntraslatedHitBox = data.Select(x => new Circle(x.relativePositionToCenter, x.radius)).ToArray();
            Radius = GetRadiusFromData(data);
        }

        private float? GetRadiusFromData((float radius, Vector2 relativePositionToCenter)[] data)
        {
            if (data.Length == 0)
                return null;

            return data.Select(x => x.relativePositionToCenter.Length() + x.radius).Max();
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
        }


        public void RenderDebug(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            
            //var point = BaseGame.debug;
            //foreach (var box in HitBox)
            //{
            //    spriteBatch.Draw(point, new Rectangle((int)(box.Center.X - box.Radius), (int)(box.Center.Y - box.Radius), (int)box.Radius * 2, (int)box.Radius * 2), Color.AliceBlue);

            //}
        }


        public bool CollidesWith(Physics other)
        {
            //if (HitBox == null || other.HitBox == null)
            //    return false;

            return (HitBox.Any(x => other.HitBox.Any(y => (x).Intersects((y)))));
        }

        public bool CollidesWith(Vector2 pos)
        {
            if (pos == null)
                return false;

            return (HitBox.Any(x => (x).Contains(pos)));
        }

    }
}
