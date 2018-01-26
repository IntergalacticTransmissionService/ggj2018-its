using Microsoft.Xna.Framework;
using System;

namespace MonoGame_Engine.Input
{
    public abstract class InputProvider : IDisposable
    {
        public abstract void Update(GameTime gameTime);

        public abstract bool Get(Buttons btn);

        public abstract float Get(Sliders sldr);

        public abstract void Rumble(float low, float high, int ms);

        public abstract void Dispose();
    }
}
