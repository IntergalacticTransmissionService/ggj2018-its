using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Entities;
using System;

namespace MonoGame_Engine.Gfx
{
    public enum AnimationType
    {
        None,
        Loop,
        OneTime
    }

    public class Image : Reloadable
    {
        private readonly string[] assetPathes;
        protected readonly Texture2D[] tex;
        private readonly TimeSpan frameSpeed;

        public AnimationType animationType { get; }

        public Vector2 origin;
        public int Width { get { return this.tex[0]?.Width ?? 0; } }
        public int Height { get { return this.tex[0]?.Height ?? 0; } }

        public int CurrentFrame => this.currentFrame;

        private int currentFrame;
        private TimeSpan frameUpdateTime;

        public Image(string assetPath) : this(TimeSpan.MaxValue, AnimationType.None, new string[] { assetPath })
        {

        }

        public Image(TimeSpan frameSpeed, AnimationType animationType, params string[] assetPath)
        {
            this.assetPathes = assetPath;
            this.tex = new Texture2D[assetPath.Length];
            this.frameSpeed = frameSpeed;
            this.animationType = animationType;
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
            spriteBatch.Draw(this.tex[currentFrame], pos, null, null, this.origin, rot);
        }

        internal virtual void Update(GameTime gameTime)
        {
            if (animationType == AnimationType.None)
                return;

            if (animationType == AnimationType.OneTime && currentFrame == tex.Length - 1)
                return;

            this.frameUpdateTime += gameTime.ElapsedGameTime;
            while (frameUpdateTime > this.frameSpeed)
            {
                this.currentFrame++;
                frameUpdateTime -= frameSpeed;
            }

            if (animationType == AnimationType.Loop)
                currentFrame %= tex.Length;
            else
                currentFrame = System.Math.Min(currentFrame, tex.Length - 1);

        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 pos, float rot, float size, Color color)
        {
            var scale = new Vector2(size / this.Width, size / this.Width);

            spriteBatch.Draw(this.tex[this.CurrentFrame], pos, null, null, this.origin, rot, scale, color);

        }
    }
}
