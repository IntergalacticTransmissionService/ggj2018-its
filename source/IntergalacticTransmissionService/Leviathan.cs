using System;
using System.Collections.Generic;
using System.Text;
using IntergalacticTransmissionService.Behaviors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Math;

namespace IntergalacticTransmissionService
{
    class Leviathan : GameObject
    {
        public Vector2 StartPos;
        public float StartRot;

        public Behavior Behavior { get; set; }
        public bool IsAlive { get; internal set; }

        private SoundEffect sndExplode;

        public Leviathan(ITSGame game, Color baseColor, float radius, Vector2 startPos, float startRot = 0, Behavior behavior = null) : base(game, new MonoGame_Engine.Gfx.Image(TimeSpan.FromSeconds(0.4), "Images/boss-1.png", "Images/boss-2.png"), "Images/boss-1.png", Color.SlateGray, baseColor, radius, false)
        {
            this.StartPos = startPos;
            this.StartRot = startRot;
            this.Behavior = behavior;
            this.IsAlive = true;
            this.HighlightIndicator = true;
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            base.LoadContent(content, wasReloaded);

            sndExplode = content.Load<SoundEffect>("Sounds/explosion");
            Phy.Pos = StartPos;
            Phy.Rot = StartRot;
        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsAlive)
            {
                base.Draw(spriteBatch, gameTime);
            }
        }

        internal override void Update(GameTime gameTime)
        {
            if (IsAlive)
            {
                base.Update(gameTime);
                Behavior?.Update(this, gameTime);

                SpawnEnemies(gameTime);
            }
        }

        private TimeSpan timeUntilNextSpawnWave = TimeSpan.FromSeconds(10);
        private int waveNumber;

        private void SpawnEnemies(GameTime gameTime)
        {
            timeUntilNextSpawnWave -= gameTime.ElapsedGameTime;
            if (timeUntilNextSpawnWave < TimeSpan.Zero)
            {
                timeUntilNextSpawnWave += TimeSpan.FromSeconds(10 + waveNumber);

                for (int i = 0; i < waveNumber * 2; ++i)
                {
                    var dist = 700;
                    var enemy = new Enemy(game,
                        Color.White,
                        RandomFuncs.FromRange(16f, 64f),
                        this.Phy.Pos + new Vector2(RandomFuncs.FromRange(-dist, dist), RandomFuncs.FromRange(-dist, dist)),
                        (float)RandomFuncs.FromRange(0, MathHelper.TwoPi),
                        new ChasingBehavior(game.MainScene, 500, 800, RandomFuncs.FromRange(100, 300)));
                    enemy.LoadContent(game.Content);
                    game.MainScene.Enemies.Add(enemy);
                }
                ++waveNumber;
            }
        }

        public void Die()
        {
            this.IsAlive = false;
            sndExplode.Play();
        }

        public void Reset(Vector2? pos = null, float? rot = null)
        {
            this.IsAlive = true;
            this.Phy.Pos = pos ?? StartPos;
            this.Phy.Rot = rot ?? StartRot;
            this.Behavior.Reset();
        }

        internal void WasHit()
        {
            //Die(); The leviathan downt die :)
        }
    }
}
