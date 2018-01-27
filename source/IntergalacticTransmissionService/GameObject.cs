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

            if (pos.X < camTopLeft.X || pos.X > camBottomRight.X || pos.Y < camTopLeft.Y || pos.Y > camBottomRight.Y)
            {
                pos.X = MathHelper.Clamp(pos.X, camTopLeft.X + 15, camBottomRight.X - 15);
                pos.Y = MathHelper.Clamp(pos.Y, camTopLeft.Y + 15, camBottomRight.Y - 15);

                spriteBatch.Draw(indicator, pos, BaseColor);
            }

            base.Draw(spriteBatch, gameTime);
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            indicator = content.Load<Texture2D>("Images/indicator.png");
            base.LoadContent(content, wasReloaded);
        }
    }
}
