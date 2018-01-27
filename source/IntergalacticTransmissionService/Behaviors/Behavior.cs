using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntergalacticTransmissionService.Behaviors
{
    abstract class Behavior
    {
        public abstract void Update(GameObject owner, GameTime gameTime);
        public abstract void Reset();
    }
}
