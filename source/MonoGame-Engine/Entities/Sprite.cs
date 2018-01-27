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
        public float Radius { get; private set; }
        public Color BaseColor { get; set;}

        public Sprite(Image img, float radius) : this(img, radius, Color.White)
        {

        }

        public Sprite(Image img, float radius, Color color)
        {
            Gfx = img;
            Radius = radius;
            BaseColor = color;
        }

        public Sprite(string assetPath, float radius, Color color) : this(new Image(assetPath), radius, color)
        {
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            Gfx.LoadContent(content, wasReloaded);
            if (Radius < 0)
                Radius = Gfx.Width * 0.5f;
            if (!wasReloaded)
            {
                Phy = new OrientedPhysics(Radius);
            }
        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Gfx.Draw(spriteBatch, Phy.Pos, Phy.Rot, Phy.HitBox.Radius, BaseColor);
        }

        internal override void Update(GameTime gameTime)
        {
            Phy.Update(gameTime);
        }
    }
}
