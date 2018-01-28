using IntergalacticTransmissionService;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine;
using MonoGame_Engine.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace IntergalacticTransmissionService
{

    public enum BulletType
    {
        Normal,
        Spread,
        Back,
        UpDown,
    }

    public class BulletSystem : Entity, IEnumerable<Vector2>
    {
        private Player player;

        private int maxParticles;
        private int active;

        private float accumulator;

        private Vector2[] pos;
        private Vector2[] spd;
        private Color[] col;
        private Color[] maxCol;

        private float[] age;

        private string asset;
        private Texture2D tex;
        private Vector2 origin;

        public float maxAgeInSecs;

        public Vector2 Pos;
        public Vector2 Spd;
        public float Dmp;

        public float SpawnRate;

        public bool Emitting { get; set; }

        public TimeSpan RapidFire { get; set; }
        private SoundEffect snd;

        public BulletSystem(Player player, string asset, int maxParticles, float spawnRateInPartPerSec)
        {
            this.player = player;
            this.asset = asset;
            this.maxParticles = maxParticles;
            this.SpawnRate = spawnRateInPartPerSec;
            this.pos = new Vector2[maxParticles];
            this.spd = new Vector2[maxParticles];

            this.col = new Color[maxParticles];
            this.maxCol = new Color[maxParticles];

            this.age = new float[maxParticles];

            Dmp = 0.92f;
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded)
        {
            tex = content.Load<Texture2D>(asset);
            snd = content.Load<SoundEffect>("Sounds/shot");
            if (!wasReloaded)
            {
                origin = new Vector2(tex.Width * 0.5f, tex.Height * 0.5f);
                maxAgeInSecs = 2;
            }
        }

        private bool Spawn(BulletType type, TimeSpan offset, bool playSound = true)
        {
            bool didSpawn = false;
            switch (type)
            {
                case BulletType.Normal:
                default:
                    didSpawn |= SpawnSingle(0f, offset);
                    break;
                case BulletType.Spread:
                    didSpawn |= SpawnSingle(0f, offset);
                    didSpawn |= SpawnSingle(-MathHelper.PiOver4 / 2, offset);
                    didSpawn |= SpawnSingle(MathHelper.PiOver4 / 2, offset);
                    break;
                case BulletType.Back:
                    didSpawn |= SpawnSingle(0f, offset);
                    didSpawn |= SpawnSingle(MathHelper.Pi, offset);
                    break;
                case BulletType.UpDown:
                    didSpawn |= SpawnSingle(MathHelper.PiOver2, offset);
                    didSpawn |= SpawnSingle(-MathHelper.PiOver2, offset);
                    break;
            }
            if (playSound && didSpawn)
                snd.Play();
            return didSpawn;
        }

        internal void Fire()
        {
            accumulator = 0;
            if(RapidFire <= TimeSpan.Zero) 
                Spawn(player.BulletType, TimeSpan.Zero);
        }

        private bool SpawnSingle(float rotation, TimeSpan offset)
        {
            if (active < maxParticles)
            {
                pos[active] = player.Phy.Pos;
                var rotationMatrix = Matrix.CreateRotationZ(player.Phy.Rot + rotation);
                spd[active] = player.Phy.Spd + Vector2.Transform(new Vector2(1, 0), rotationMatrix) * 2000;
                if(offset > TimeSpan.Zero)
                    pos[active] += spd[active]*(float)offset.TotalSeconds;
                col[active] = player.BaseColor;
                maxCol[active] = col[active];
                maxCol[active].A = 0;
                age[active] = 0;
                active++;
                return true;
            }
            return false;
        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < active; ++i)
            {
                var _col = Color.Lerp(col[i], maxCol[i], age[i] / maxAgeInSecs);
                spriteBatch.Draw(tex, Pos + pos[i], null, null, origin, 0, Vector2.One, _col);
            }
        }

        internal override void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Pos += Spd * delta;

            
            if (RapidFire.Ticks > 0 && Emitting)
            {
                accumulator += delta;
                RapidFire -= gameTime.ElapsedGameTime;
                var numberOfParticlesFired = 0;
                var reduce = 1 / SpawnRate;
                while (accumulator > 0)
                {
                    if (!Spawn(player.BulletType, TimeSpan.FromSeconds(accumulator), false))
                        break;
                    accumulator -= reduce;
                    numberOfParticlesFired++;
                }
                if (numberOfParticlesFired > 0)
                    snd.Play();
            }
            //DebugOverlay.Instance.Text += $"RapidFire ({this.player.PlayerNum}): {RapidFire}\n";

            // Update
            for (int i = 0; i < active; ++i)
            {
                pos[i] += spd[i] * delta * Dmp;
                age[i] += delta;
            }

            // kill old particles by copying in from end
            for (int i = 0; i < active; ++i)
            {
                if (age[i] > maxAgeInSecs)
                {
                    pos[i] = pos[active - 1];
                    spd[i] = spd[active - 1];
                    col[i] = col[active - 1];
                    maxCol[i] = maxCol[active - 1];
                    age[i] = age[active - 1];
                    --i;
                    --active;
                }
            }
        }

        public void Remove(int i)
        {
            age[i] = maxAgeInSecs + 1;
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            for (int i = 0; i < active; ++i)
                yield return pos[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < active; ++i)
                yield return pos[i];
        }
    }
}
