using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntergalacticTransmissionService
{
    public class Parcel : EntityWithIndicator
    {
        public Player LastHeldBy { get; set; }
        public Player HoldBy { get; set; }
        public TimeSpan Cooldown { get; private set; }

        private Texture2D img;
        private Vector2 origin;

        public Parcel(ITSGame game, Color color, float radius) : base(game, color, radius)
        {
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            base.LoadContent(content, wasReloaded);
            img = content.Load<Texture2D>("Images/parcel.png");
            origin = new Vector2(img.Width * 0.5f, img.Height * 0.5f);

            if (!wasReloaded)
            {
                Phy.Dmp = 0.95f;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            var scale = new Vector2(Phy.HitBox.Radius / (img.Width * 0.5f), Phy.HitBox.Radius / (img.Height * 0.5f));
            spriteBatch.Draw(img, Phy.Pos, null, null, origin, Phy.Rot, scale, BaseColor);
        }

        internal override void Update(GameTime gameTime)
        {
            if (HoldBy == null && Cooldown > TimeSpan.Zero)
                Cooldown -= gameTime.ElapsedGameTime;

            if (HoldBy != null && Cooldown <= TimeSpan.Zero)
            {
                Phy.Pos = HoldBy.Phy.Pos;
            }
            base.Update(gameTime);
        }

        internal void Grab(Player player)
        {
            if (HoldBy == null && (Cooldown <= TimeSpan.Zero || player != LastHeldBy))
            {
                HoldBy = player;
                Phy.Spd = Vector2.Zero;
                Cooldown = TimeSpan.Zero;
            } else if (player != LastHeldBy)
            {
                Phy.Spd *= 0.5f;
            }
        }

        internal void Release(Player player, float power = 4000)
        {
            if (player == HoldBy)
            {
                LastHeldBy = HoldBy;
                HoldBy = null;
                Cooldown = TimeSpan.FromSeconds(1);
                Phy.Spd = player.Phy.Spd + Vector2.Normalize(player.Phy.Spd) * power;
            }
        }
    }
}
