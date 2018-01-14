using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Entities;

namespace MonoGame_Engine.Gfx
{
    public class Image : Reloadable
    {
        private readonly string assetPath;
        protected Texture2D tex;

        public Vector2 origin;

        public Image(string assetPath)
        {
            this.assetPath = assetPath;
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded)
        {
            this.tex = content.Load<Texture2D>(assetPath);
            if (!wasReloaded)
            {
                this.origin = new Vector2(tex.Width * 0.5f, tex.Height * 0.5f);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 pos, float rot)
        {
            spriteBatch.Draw(tex, pos, null, null, origin, rot);
        }
    }
}
