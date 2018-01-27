using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Entities;
using MonoGame_Engine.Phy;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntergalacticTransmissionService
{
    public class GameObject : Sprite
    {
        protected readonly ITSGame game;

        private Texture2D indicator;
        private Vector2 origin;
        private float Scale = 0.2f;

        public Color IndicatorColor;
        public string IndicatorAsset;
        public Texture2D IndicatorTex;

        public bool HighlightIndicator;

        public virtual float IndicatorLargeScale { get { return 1.2f; } }
        public virtual float IndicatorSmallScale { get { return 0.4f; } }

        public GameObject(ITSGame game, string assetPath, string indicatorAssetPath, Color indicatorColor, Color baseColor, float radius, bool orientedPhysics = true) : base(assetPath, radius, baseColor, orientedPhysics)
        {
            this.game = game;
            this.IndicatorColor = indicatorColor;
            this.IndicatorAsset = indicatorAssetPath;
        }


        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var pos = new Vector2();
            pos.X = Phy.Pos.X;
            pos.Y = Phy.Pos.Y;

            var camTopLeft = game.Camera.TopLeft;
            var camBottomRight = game.Camera.BottomRight;

            float checkRadius = Radius * 0.5f;

            if (pos.X + checkRadius < camTopLeft.X || pos.X - checkRadius > camBottomRight.X || pos.Y + checkRadius < camTopLeft.Y || pos.Y - checkRadius > camBottomRight.Y)
            {
                pos.X = MathHelper.Clamp(pos.X, camTopLeft.X + 3, camBottomRight.X - 3);
                pos.Y = MathHelper.Clamp(pos.Y, camTopLeft.Y + 3, camBottomRight.Y - 3);

                var dir = Vector2.Normalize(Phy.Pos - game.Camera.Phy.Pos);
                var rot = (float)Math.Atan2(dir.Y, dir.X) + MathHelper.PiOver2;

                if (IndicatorTex != null)
                {
                    var pos2 = pos - (dir * 80 * Scale);
                    var origin = new Vector2(IndicatorTex.Width * 0.5f, IndicatorTex.Height * 0.5f);
                    var scale2 = (32f / IndicatorTex.Width);
                    spriteBatch.Draw(IndicatorTex, pos2, null, Color.White, 0, origin, Scale * scale2, SpriteEffects.None, 0);
                }

                spriteBatch.Draw(indicator, pos, null, IndicatorColor, rot, origin, Scale, SpriteEffects.None, 0);
            }

            base.Draw(spriteBatch, gameTime);
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Scale = HighlightIndicator ? IndicatorLargeScale : IndicatorSmallScale;
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            indicator = content.Load<Texture2D>("Images/indicator.png");
            origin = new Vector2(indicator.Width * 0.5f, 10f);

            if (!string.IsNullOrEmpty(IndicatorAsset))
                IndicatorTex = content.Load<Texture2D>(IndicatorAsset);
            else
                IndicatorTex = null;

            base.LoadContent(content, wasReloaded);
        }
    }
}
