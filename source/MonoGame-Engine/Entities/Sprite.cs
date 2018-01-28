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
        public Color BaseColor { get; set; }

        private readonly bool orientedPhysics;

        public Sprite(Image img, float radius, bool orientedPhysics = true) : this(img, radius, Color.White, orientedPhysics)
        {

        }

        public Sprite(Image img, float radius, Color color, bool orientedPhysics = true)
        {
            Gfx = img;
            Radius = radius;
            BaseColor = color;
            this.orientedPhysics = orientedPhysics;
        }

        public Sprite(string assetPath, float radius, Color color, bool orientedPhysics = true) : this(new Image(assetPath), radius, color, orientedPhysics)
        {
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            Gfx.LoadContent(content, wasReloaded);
            if (Radius < 0)
                Radius = Gfx.Width * 0.5f;
            if (!wasReloaded)
            {
                Phy = InitilisePhysics();
            }
        }

        protected virtual Physics InitilisePhysics() => this.orientedPhysics ? new OrientedPhysics(this.Radius) : new Physics(this.Radius);

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Gfx.Draw(spriteBatch, Phy.Pos, Phy.Rot, this.Radius, BaseColor);
            //Phy.RenderDebug(spriteBatch);
        }

        internal override void Update(GameTime gameTime)
        {
            Phy.Update(gameTime);
            Gfx.Update(gameTime);
        }
    }
}
