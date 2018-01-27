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
        private float scale = 3;

        public GameObject(ITSGame game, string assetPath, Color baseColor, float radius) : base(assetPath, radius, baseColor)
        {
            this.game = game;
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
                pos.X = MathHelper.Clamp(pos.X, camTopLeft.X + 15, camBottomRight.X - 15);
                pos.Y = MathHelper.Clamp(pos.Y, camTopLeft.Y + 15, camBottomRight.Y - 15);

                spriteBatch.Draw(indicator, pos, null, BaseColor,0, origin, scale, SpriteEffects.None, 0);
            }

            base.Draw(spriteBatch, gameTime);
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            indicator = content.Load<Texture2D>("Images/indicator.png");
            origin = new Vector2(indicator.Width * 0.5f, indicator.Height * 0.5f);
            base.LoadContent(content, wasReloaded);
        }
    }
}
