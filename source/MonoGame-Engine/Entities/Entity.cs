using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Gfx;

namespace MonoGame_Engine.Entities
{
    public abstract class Entity : Reloadable
    {
        internal abstract void Update(GameTime gameTime);

        internal abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
