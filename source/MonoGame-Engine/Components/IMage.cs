using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Engine.Engine.Components
{
    public class Image
    {
        private readonly string assetPath;
        protected Texture2D tex;

        public Vector2 origin;

        public Image(string assetPath)
        {
            this.assetPath = assetPath;
        }

        public void LoadContent(ContentManager content)
        {
            this.tex = content.Load<Texture2D>(assetPath);
            this.origin = new Vector2(tex.Width * 0.5f, tex.Height * 0.5f);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 pos, float rot)
        {
            spriteBatch.Draw(tex, pos, null, null, origin, rot);
        }
    }
}
