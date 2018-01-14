using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame_Engine.Phy;
using MonoGame_Engine.Gfx;

namespace MonoGame_Engine.Entities
{
    public class Sprite : Entity, IHasPhysics
    {
        public Physics Phy { get; private set; }
        public Image Gfx { get; private set; }

        public Sprite(Image img)
        {
            Gfx = img;
        }

        public Sprite(string assetPath)
        {
            Gfx = new Image(assetPath);
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            Gfx.LoadContent(content, wasReloaded);
            if (!wasReloaded)
            {
                Phy = new Physics(Gfx.origin.X);
            }
        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Gfx.Draw(spriteBatch, Phy.Pos, Phy.Rot);
        }

        internal override void Update(GameTime gameTime)
        {
            Phy.Update(gameTime);
        }
    }
}
