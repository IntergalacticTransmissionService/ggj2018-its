using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine;
using MonoGame_Engine.Entities;
using MonoGame_Engine.Phy;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MonoGame_Engine.Gfx;

namespace IntergalacticTransmissionService
{
    public class Player : GameObject
    {
        public const float DefaultMaxSpd = 800f;
        public float MaxSpd { get { return game.MainScene.Parcel.HoldBy == this ? DefaultMaxSpd * 0.9f : DefaultMaxSpd; } }

        public BulletSystem Bullets { get; private set; }
        public Image flame { get; }

        public int PlayerNum { get; private set; }

        public bool IsAlive { get; private set; }
        public bool IsInvincible { get { return InvincibleCooldown > TimeSpan.Zero; } }
        public BulletType BulletType { get; private set; }

        public TimeSpan RespawnCooldown;
        public TimeSpan InvincibleCooldown;
        public readonly Dictionary<CollectableType, int> Collectables;

        public static Color[] colors = new Color[] {
            new Color(0xF3, 0x00, 0x28),    // Red
            new Color(0x00, 0x28, 0xF3),    // Blue
            new Color(0x00, 0x88, 0x3F),    // Green    
            new Color(0xBD, 0x0A, 0x7B)     // Purple
        };

        public Player(ITSGame game, int playerNum, float radius) : base(game, "Images/player.png", $"Images/character_{(playerNum % 4)+1:00}.png", colors[playerNum % colors.Length], colors[playerNum % colors.Length], radius)
        {
            this.PlayerNum = playerNum;
            Collectables = new Dictionary<CollectableType, int>();
            foreach(CollectableType e in Enum.GetValues(typeof(CollectableType)))
                Collectables.Add(e, 0);
            Bullets = new BulletSystem(this, "Images/bullet.png", 300, 15);
            IsAlive = true;
            BulletType = BulletType.Normal;
            this.flame = new MonoGame_Engine.Gfx.Image(TimeSpan.FromSeconds(0.2), "Images/PlayerFlame-1.png", "Images/PlayerFlame-2.png");

        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            base.LoadContent(content, wasReloaded);
            Bullets.LoadContent(content, wasReloaded);
            flame.LoadContent(content, wasReloaded);

        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsAlive)
            {
                if (IsInvincible)
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds % 300 < 150)
                    {
                        BaseColor = new Color(BaseColor, 0.5f);
                    } else
                    {
                        BaseColor = new Color(BaseColor, 1.0f);
                    }
                } else
                {
                    if (BaseColor.A < 255)
                        BaseColor = new Color(BaseColor, 1.0f);
                }
                base.Draw(spriteBatch, gameTime);
            }
            Bullets.Draw(spriteBatch, gameTime);
            flame.Draw(spriteBatch, Phy.Pos, Phy.Rot, Phy.HitBox.Radius, Color.White);

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
            if (IsInvincible)
            {
                InvincibleCooldown -= gameTime.ElapsedGameTime;
            }

            if (IsAlive)
            {
                Phy.Dmp = 0.95f;

                base.Update(gameTime);

                // Ensure MaxSpd
                var spd = Phy.Spd.Length();
                if (spd > MaxSpd)
                    Phy.Spd = Vector2.Normalize(Phy.Spd) * MaxSpd;
            }
            else
            {
                if (RespawnCooldown > TimeSpan.Zero)
                    RespawnCooldown -= gameTime.ElapsedGameTime;
            }

            foreach(CollectableType e in Enum.GetValues(typeof(CollectableType)))
            {
                switch (e)
                {
                    case CollectableType.RapidFire:
                        while (Collectables[e] > 0)
                        {
                            Bullets.RapidFire += TimeSpan.FromSeconds(10);
                            Collectables[e]--;
                        }
                        break;
                    case CollectableType.SpreadShoot:
                        if (Collectables[e] > 0)
                        {
                            Collectables[e] = 0;
                            BulletType = BulletType.Spread;
                        }
                        break;
                    case CollectableType.BackShoot:
                        if (Collectables[e] > 0)
                        {
                            Collectables[e] = 0;
                            BulletType = BulletType.Back;
                        }
                        break;
                    case CollectableType.UpDownShoot:
                        if (Collectables[e] > 0)
                        {
                            Collectables[e] = 0;
                            this.BulletType = BulletType.UpDown;
                        }
                        break;
                    default:
                        break;
                }
            }

            Bullets.Update(gameTime);
            flame.Update(gameTime);


            //game.DebugOverlay.Text += String.Join("  ", Enum.GetValues(typeof(CollectibleType)).Cast<CollectibleType>().Select(c => $"{c}: {this.Collectables[c]}").ToArray()) + "\n";
        }

        internal void WhereAmI(bool show)
        {
            HighlightIndicator = show;
        }

        internal void Spawn()
        {
            if (RespawnCooldown <= TimeSpan.Zero)
            {
                var rnd = new Random();
                IsAlive = true;
                Phy.Pos.X = game.Camera.Phy.Pos.X + (float)(rnd.NextDouble() - 0.5) * 500;
                Phy.Pos.Y = game.Camera.Phy.Pos.Y + (float)(rnd.NextDouble() - 0.5) * 500;
                Phy.Spd = Vector2.Zero;
                Phy.Accel = Vector2.Zero;
                RespawnCooldown = TimeSpan.Zero;
                InvincibleCooldown = TimeSpan.FromSeconds(4);
            }
        }

        public void Die()
        {
            WasHit();
            if (!IsInvincible)
            {
                Shoot(false);
                IsAlive = false;
                RespawnCooldown = TimeSpan.FromSeconds(1);
            }
        }
    }
}
