using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Math;

namespace MonoGame_Engine.Phy
{
    public class OrientedPhysics : Physics
    {
        public OrientedPhysics(float? radius) : base(radius)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Spd.Length() > 2)
                Rot = (float)Atan2(Spd.Y, Spd.X);
        }

    }
}
