using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Entities;
using MonoGame_Engine.Gfx;
using MonoGame_Engine.Phy;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntergalacticTransmissionService
{
    public class GameObject : Sprite
    {
        protected readonly ITSGame game;

        public Image flame { get; }

        private Texture2D indicator;
        private Vector2 origin;
        private float Scale = 5;

        public bool HighlightIndicator;

        public GameObject(ITSGame game, MonoGame_Engine.Gfx.Image image, Color baseColor, float radius, bool orientedPhysics = true) : base(image, radius, baseColor, orientedPhysics)
        {
            this.game = game;
            this.flame = new MonoGame_Engine.Gfx.Image(TimeSpan.FromSeconds(0.2), "Images/PlayerFlame-1.png", "Images/PlayerFlame-2.png");
        }
        public GameObject(ITSGame game, string assetPath, Color baseColor, float radius, bool orientedPhysics = true) : base(assetPath, radius, baseColor, orientedPhysics)
        {
            this.game = game;
            this.flame = new MonoGame_Engine.Gfx.Image(TimeSpan.FromSeconds(0.2), "Images/PlayerFlame-1.png", "Images/PlayerFlame-2.png");
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

                spriteBatch.Draw(indicator, pos, null, BaseColor, 0, origin, Scale, SpriteEffects.None, 0);
            }

            flame.Draw(spriteBatch, Phy.Pos, Phy.Rot, Phy.HitBox.Radius, Color.White);
            base.Draw(spriteBatch, gameTime);
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            flame.Update(gameTime);
            Scale = HighlightIndicator ? 3 : 1;
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            indicator = content.Load<Texture2D>("Images/indicator.png");
            origin = new Vector2(indicator.Width * 0.5f, indicator.Height * 0.5f);
            flame.LoadContent(content, wasReloaded);
            base.LoadContent(content, wasReloaded);
        }
    }
}
