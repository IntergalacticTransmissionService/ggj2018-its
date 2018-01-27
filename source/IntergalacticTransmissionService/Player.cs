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

namespace IntergalacticTransmissionService
{
    public class Player : GameObject
    {
        public const float MaxSpd = 800f;

        public BulletSystem Bullets { get; private set; }

        public int PlayerNum { get; private set; }

        public bool IsAlive { get; private set; }
        public BulletType BulletType { get; private set; }

        public TimeSpan Cooldown;
        public readonly int[] Collectables;

        public static Color[] colors = new Color[] {
            new Color(0xCC, 0x00, 0x00),    // Red
            new Color(0x44, 0xFF, 0x00),    // Green
            new Color(0x00, 0x44, 0xff),    // Blue    
            new Color(0xFF, 0xCC, 0x00),    // Yellow
            new Color(0xCC, 0x00, 0x88)     // Purple
        };

        public Player(ITSGame game, int playerNum, float radius) : base(game, "Images/player.png", colors[playerNum % colors.Length], radius)
        {
            this.PlayerNum = playerNum;
            Collectables = new int[Enum.GetValues(typeof(CollectibleType)).Length];
            Bullets = new BulletSystem(this, "Images/bullet.png", 300, 15);
            IsAlive = true;
            BulletType = BulletType.Normal;
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            base.LoadContent(content, wasReloaded);
            Bullets.LoadContent(content, wasReloaded);
        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsAlive)
            {
                base.Draw(spriteBatch, gameTime);
            }
            Bullets.Draw(spriteBatch, gameTime);
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
                if (Cooldown > TimeSpan.Zero)
                    Cooldown -= gameTime.ElapsedGameTime;
            }

            for (int i = 0; i < Collectables.Length; i++)
            {
                ref int count = ref Collectables[i];
                switch ((CollectibleType)i)
                {
                    case CollectibleType.RapidFire:
                        while (count > 0)
                        {
                            Bullets.RapidFire += TimeSpan.FromSeconds(10);
                            count--;
                        }
                        break;
                    case CollectibleType.SpreadShoot:
                        if (count > 0)
                        {
                            count = 0;
                            BulletType = BulletType.Spread;
                        }
                        break;
                    case CollectibleType.BackShoot:
                        if (count > 0)
                        {
                            count = 0;
                            BulletType = BulletType.Back;
                        }
                        break;
                    case CollectibleType.UpDownShoot:
                        if (count > 0)
                        {
                            count = 0;
                            this.BulletType = BulletType.UpDown;
                        }
                        break;
                    default:
                        break;
                }
            }

            Bullets.Update(gameTime);


            game.DebugOverlay.Text += String.Join("  ", Enum.GetValues(typeof(CollectibleType)).Cast<int>().Select(c => $"{(CollectibleType)c}: {this.Collectables[c]}").ToArray()) + "\n";
        }

        internal void WhereAmI(bool show)
        {
            HighlightIndicator = show;
        }

        internal void Spawn()
        {
            if (Cooldown <= TimeSpan.Zero)
            {
                var rnd = new Random();
                IsAlive = true;
                Phy.Pos.X = game.Camera.Phy.Pos.X + (float)(rnd.NextDouble() - 0.5) * 500;
                Phy.Pos.Y = game.Camera.Phy.Pos.Y + (float)(rnd.NextDouble() - 0.5) * 500;
                Phy.Spd = Vector2.Zero;
                Phy.Accel = Vector2.Zero;
            }
        }

        public void Die()
        {
            Shoot(false);
            IsAlive = false;
            Cooldown = TimeSpan.FromSeconds(3);
        }
    }
}
