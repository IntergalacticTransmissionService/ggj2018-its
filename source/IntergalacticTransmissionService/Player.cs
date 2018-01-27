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
    public class Player : EntityWithIndicator, IHasPhysics
    {

        private Texture2D halo;
        private Vector2 haloOrigin;


        public BulletSystem Bullets { get; private set; }

        public int PlayerNum { get; private set; }

        public static Color[] colors = new Color[] {
            new Color(0xCC, 0x00, 0x00),    // Red
            new Color(0x44, 0xFF, 0x00),    // Green
            new Color(0x00, 0x44, 0xff),    // Blue    
            new Color(0xFF, 0xCC, 0x00),    // Yellow
            new Color(0xCC, 0x00, 0x88)     // Purple
        };

        public Player(ITSGame game, int playerNum, float radius) : base(game, colors[playerNum % colors.Length], radius)
        {
            this.PlayerNum = playerNum;
            Bullets = new BulletSystem(this, "Images/bullet.png", 300, 15);
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            base.LoadContent(content, wasReloaded);
            halo = content.Load<Texture2D>("Images/player.png");
            Bullets.LoadContent(content, wasReloaded);
            if (!wasReloaded)
            {
                haloOrigin = new Vector2(halo.Width * 0.5f, halo.Height * 0.5f);
            }
        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            Bullets.Draw(spriteBatch, gameTime);

            var pos = new Vector2();
            pos.X = Phy.Pos.X;
            pos.Y = Phy.Pos.Y;

            var scale = new Vector2(Phy.HitBox.Radius / (halo.Width * 0.5f), Phy.HitBox.Radius / (halo.Height * 0.5f));

            spriteBatch.Draw(halo, pos, null, null, haloOrigin, Phy.Rot, scale, BaseColor);
        }

        internal void ReleaseParcel()
        {
            game.MainScene.Parcel.Release(this);
        }

        public void WasHit()
        {
            game.Inputs.Player(PlayerNum).Rumble(160, 320, 200);
        }

        public void Shoot(bool active)
        {
            Bullets.Emitting = active;
        }

        internal override void Update(GameTime gameTime)
        {
            Phy.Dmp = 0.95f;

            base.Update(gameTime);

            // Ensure MaxSpd
            var spd = Phy.Spd.Length();
            if (spd > 1200)
                Phy.Spd = Vector2.Normalize(Phy.Spd) * 1200.0f;

            Bullets.Update(gameTime);
        }
    }
}
