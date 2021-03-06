﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IntergalacticTransmissionService.Behaviors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Gfx;
using MonoGame_Engine.Math;
using MonoGame_Engine.Phy;

namespace IntergalacticTransmissionService
{
    class Leviathan : GameObject
    {
        public Vector2 StartPos;
        public float StartRot;

        public LeviathanBehavior Behavior { get; set; }
        public bool IsAlive { get; internal set; }
        public bool IsFleeing { get; set; }

        private readonly Image astronaut;
        private readonly float zoomSpeed = 0.1f;
        float astronatZoom = 0f;
        Vector2 astronautOffset;

        private SoundEffect sndExplode;

        private int health;

        public Leviathan(ITSGame game, Color baseColor, float radius, Vector2 startPos, float startRot = 0, LeviathanBehavior behavior = null) : base(game, new MonoGame_Engine.Gfx.Image(TimeSpan.FromSeconds(0.4), AnimationType.Loop, "Images/boss-1.png", "Images/boss-2.png"), "Images/boss-1.png", Color.SlateGray, baseColor, radius, false)
        {
            this.StartPos = startPos;
            this.StartRot = startRot;
            this.Behavior = behavior;
            this.IsAlive = true;
            this.IsFleeing = false;
            this.HighlightIndicator = true;
            health = 500;
            astronaut = new Image("Images/happy-ending.png");
        }

        protected override Physics InitilisePhysics()
        {
            var xValues = new float[] { 120, 60, 0, -60, -120, -180 };
            var yValues = new float[] { 200, 140, 80, 20, 0, -20, -80, -140, -200, -260, -320 };


            const float r = 30f;

            return new Physics(xValues.SelectMany(x => yValues.Select(y => (r, new Vector2(x, y)))).ToArray());
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            base.LoadContent(content, wasReloaded);

            sndExplode = content.Load<SoundEffect>("Sounds/explosion");
            Phy.Pos = StartPos;
            Phy.Rot = StartRot;
            astronaut.LoadContent(content, wasReloaded);

        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsAlive)
            {
                base.Draw(spriteBatch, gameTime);
            }
            else
            {
                astronaut.Draw(spriteBatch, this.Phy.Pos + astronautOffset, astronautRotation, MathHelper.Clamp(astronatZoom * astronaut.Width, 0, astronaut.Width), Color.White);
            }
        }

        internal override void Update(GameTime gameTime)
        {
            if (IsAlive)
            {
                base.Update(gameTime);
                if (Behavior != null)
                    Behavior.LeviathanSpeedFactor = IsFleeing ? 0.5f : 0.05f;
                Behavior?.Update(this, gameTime);
                SpawnEnemies(gameTime);
            }
            else
            {
                astronaut.Update(gameTime);
                astronatZoom += zoomSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                var up = ((float)gameTime.TotalGameTime.TotalSeconds % 4) / 4;
                astronautRotation = MathHelper.TwoPi * up;


            }
        }

        private TimeSpan timeUntilNextSpawnWave = TimeSpan.FromSeconds(6);
        private int waveNumber;
        private float astronautRotation;

        private void SpawnEnemies(GameTime gameTime)
        {
            timeUntilNextSpawnWave -= gameTime.ElapsedGameTime;
            if (timeUntilNextSpawnWave < TimeSpan.Zero)
            {
                timeUntilNextSpawnWave += TimeSpan.FromSeconds(6);

                for (int i = 0; i < waveNumber * 2; ++i)
                {
                    var dist = 20;
                    var enemy = new Enemy(game,
                        Color.White,
                        RandomFuncs.FromRange(16f, 64f),
                        this.Phy.Pos + new Vector2(RandomFuncs.FromRange(-dist, dist), RandomFuncs.FromRange(-dist, dist)),
                        (float)RandomFuncs.FromRange(0, MathHelper.TwoPi),
                        new ChasingBehavior(game.MainScene, 500, 1500, RandomFuncs.FromRange(Player.DefaultMaxSpd * 0.2f, Player.DefaultMaxSpd * 0.7f)));
                    enemy.LoadContent(game.Content);
                    enemy.Phy.Spd = new Vector2(
                        RandomFuncs.FromRange(-Player.DefaultMaxSpd * 0.5f, Player.DefaultMaxSpd * 0.5f),
                        RandomFuncs.FromRange(-Player.DefaultMaxSpd * 0.5f, Player.DefaultMaxSpd * 0.5f));
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

        public void Start()
        {
            IsFleeing = true;
        }

        public void Reset(Vector2? pos = null, float? rot = null)
        {
            this.IsAlive = true;
            this.Phy.Pos = pos ?? StartPos;
            this.Phy.Rot = rot ?? StartRot;
            this.Behavior.Reset();
        }

        internal void WasHit(bool hitByPackage)
        {
            health -= hitByPackage ? 100 : 1;
            sndExplode.Play();
            if (health <= 0)
                Die();
        }
    }
}
