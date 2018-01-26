using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine;
using MonoGame_Engine.Entities;
using MonoGame_Engine.Phy;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntergalacticTransmissionService
{
    public class Player : Entity, IHasPhysics
    {
        private readonly BaseGame game;

        private Texture2D halo;
        private Vector2 haloOrigin;

        private Texture2D indicator;

        public Color BaseColor { get; private set; }

        public Physics Phy { get; private set; }

        public float Radius
        {
            get { return this.Phy.HitBox.Radius; }
            set { this.Phy.HitBox.Radius = value; }
        }

        public int PlayerNum { get; private set; }

        public static Color[] colors = new Color[] {
            new Color(0xCC, 0x00, 0x00),    // Red
            new Color(0x44, 0xFF, 0x00),    // Green
            new Color(0x00, 0x44, 0xff),    // Blue    
            new Color(0xFF, 0xCC, 0x00),    // Yellow
            new Color(0xCC, 0x00, 0x88)     // Purple
        };

        public Player(BaseGame game, int playerNum, float radius)
        {
            this.game = game;
            this.PlayerNum = playerNum;
            this.BaseColor = colors[playerNum % colors.Length];
            Phy = new OrientedPhysics(radius);
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            halo = content.Load<Texture2D>("Images/halo.png");
            indicator = content.Load<Texture2D>("Images/particle.png");
            if (!wasReloaded)
            {
                haloOrigin = new Vector2(halo.Width * 0.5f, halo.Height * 0.5f);
            }
        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var pos = new Vector2();
            pos.X = Phy.Pos.X;
            pos.Y = Phy.Pos.Y;

            var scale = new Vector2(Phy.HitBox.Radius / (halo.Width * 0.5f), Phy.HitBox.Radius / (halo.Height * 0.5f));

            spriteBatch.Draw(halo, pos, null, null, haloOrigin, 0, scale, BaseColor);

            var camTopLeft = game.Camera.TopLeft;
            var camBottomRight = game.Camera.BottomRight;

            if (pos.X < camTopLeft.X || pos.X > camBottomRight.X || pos.Y < camTopLeft.Y || pos.Y > camBottomRight.Y)
            {
                pos.X = MathHelper.Clamp(pos.X, camTopLeft.X + 15, camBottomRight.X - 15);
                pos.Y = MathHelper.Clamp(pos.Y, camTopLeft.Y + 15, camBottomRight.Y - 15);

                spriteBatch.Draw(indicator, pos, BaseColor);
            }
        }

        public void WasHit()
        {
            game.Inputs.Player(PlayerNum).Rumble(160, 320, 200);
        }

        internal override void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Phy.Dmp = 0.95f;

            // Integrate
            Phy.Update(gameTime);

            // Ensure Physics Bounds
            //if (Phy.Pos.X < Radius) Phy.Pos.X = Radius;
            //if (Phy.Pos.X > game.Screen.CanvasWidth - Radius) Phy.Pos.X = game.Screen.CanvasWidth - Radius;
            //if (Phy.Pos.Y < Radius) Phy.Pos.Y = Radius;
            //if (Phy.Pos.Y > game.Screen.CanvasHeight - Radius) Phy.Pos.Y = game.Screen.CanvasHeight - Radius;

            // Ensure MaxSpd
            var spd = Phy.Spd.Length();
            if (spd > 1200)
                Phy.Spd = Vector2.Normalize(Phy.Spd) * 1200.0f;

            //if (Phy.Spd.X > 1200) Phy.Spd.X = 1200;
            //if (Phy.Spd.X < -1200) Phy.Spd.X = -1200;
            //if (Phy.Spd.Y > 1200) Phy.Spd.Y = 1200;
            //if (Phy.Spd.Y < -1200) Phy.Spd.Y = -1200;
        }
    }
}
