using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Engine.Entities
{
    public class Group : Entity
    {
        public readonly List<Entity> Children;

        public Group()
        {
            Children = new List<Entity>();
        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var c in Children)
                c.Draw(spriteBatch, gameTime);
        }

        internal override void Update(GameTime gameTime)
        {
            foreach (var c in Children)
                c.Update(gameTime);
        }
    }
}
