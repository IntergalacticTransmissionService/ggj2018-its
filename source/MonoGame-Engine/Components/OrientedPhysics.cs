using Microsoft.Xna.Framework;
using MonoGame_Engine.Engine.Components;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Math;

namespace MonoGame_Engine.Components
{
    public class OrientedPhysics : Physics
    {
        public OrientedPhysics(float? radius) : base(radius)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Rot = (float)Atan2(Spd.Y, Spd.X);
        }

    }
}
