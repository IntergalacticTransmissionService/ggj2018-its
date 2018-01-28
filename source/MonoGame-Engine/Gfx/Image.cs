using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Entities;
using System;

namespace MonoGame_Engine.Gfx
{
    public class Image : Reloadable
    {
        private readonly string[] assetPathes;
        protected readonly Texture2D[] tex;
        private readonly TimeSpan frameSpeed;
        public Vector2 origin;
        public int Width { get { return this.tex[0]?.Width ?? 0; } }
        public int Height { get { return this.tex[0]?.Height ?? 0; } }

        public int CurrentFrame => this.currentFrame;

        private int currentFrame;
        private TimeSpan frameUpdateTime;

        public Image(string assetPath) : this(TimeSpan.MaxValue, new string[] { assetPath })
        {

        }

        public Image(TimeSpan frameSpeed, params string[] assetPath)
        {
            this.assetPathes = assetPath;
            this.tex = new Texture2D[assetPath.Length];
            this.frameSpeed = frameSpeed;
        }


        internal override void LoadContent(ContentManager content, bool wasReloaded)
        {
            for (int i = 0; i < this.assetPathes.Length; i++)
            {
                this.tex[i] = content.Load<Texture2D>(this.assetPathes[i]);
            }
            if (!wasReloaded)
            {
                this.origin = new Vector2(this.tex[0].Width * 0.5f, this.tex[0].Height * 0.5f);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 pos, float rot)
        {
            spriteBatch.Draw(this.tex[0], pos, null, null, this.origin, rot);
        }

        internal virtual void Update(GameTime gameTime)
        {
            this.frameUpdateTime += gameTime.ElapsedGameTime;
            while (frameUpdateTime > this.frameSpeed)
            {
                this.currentFrame++;
                frameUpdateTime -= frameSpeed;
            }
            currentFrame %= tex.Length;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 pos, float rot, float size, Color color)
        {
            var scale = new Vector2(size / this.Width, size / this.Height);

            spriteBatch.Draw(this.tex[this.CurrentFrame], pos, null, null, this.origin, rot, scale, color);

        }
    }
}
