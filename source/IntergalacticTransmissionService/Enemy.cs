using System;
using System.Collections.Generic;
using System.Text;
using IntergalacticTransmissionService.Behaviors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Gfx;

namespace IntergalacticTransmissionService
{
    class Enemy : GameObject
    {
        public Vector2 StartPos;
        public float StartRot;

        public Behavior Behavior { get; set; }
        public bool IsAlive { get; internal set; }

        private SoundEffect sndExplode;

        public override float IndicatorSmallScale => 0.25f;

        public Enemy(ITSGame game, Color baseColor, float radius, Vector2 startPos, float startRot = 0, Behavior behavior = null) : base(game, GenerateImage(), null, new Color(0.8f, 0.8f, 0.8f, 0.4f), baseColor, radius)
        {
            this.StartPos = startPos;
            this.StartRot = startRot;
            this.Behavior = behavior;
            this.IsAlive = true;
        }

        private static Image GenerateImage()
        {
            if (MonoGame_Engine.Math.RandomFuncs.FromRangeInt(0, 1) == 0)
                return new Image(TimeSpan.FromSeconds(0.25f), "Images/enemy2-1.png", "Images/enemy2-2.png");
            else
                return new Image(TimeSpan.FromSeconds(0.25f), "Images/enemy1-1.png", "Images/enemy1-2.png");
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
            Die();
        }
    }
}
