using Microsoft.Xna.Framework;

namespace MonoGame_Engine.Engine.Input
{
    public abstract class InputProvider
    {
        public abstract void Update(GameTime gameTime);

        public abstract bool Get(Buttons btn);

        public abstract float Get(Sliders sldr);

        public abstract void Rumble(float low, float high, int ms);
    }
}
