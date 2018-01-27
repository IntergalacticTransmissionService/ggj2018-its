using System;
using System.Collections.Generic;
using System.Text;
using IntergalacticTransmissionService.Behaviors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace IntergalacticTransmissionService
{
    class Enemy : GameObject
    {
        public Vector2 StartPos;
        public float StartRot;

        public Behavior Behavior { get; set; }
        public bool IsAlive { get; internal set; }

        public Enemy(ITSGame game, Color baseColor, float radius, Vector2 startPos, float startRot = 0, Behavior behavior = null) : base(game, "Images/enemy.png", baseColor, radius)
        {
            this.StartPos = startPos;
            this.StartRot = startRot;
            this.Behavior = behavior;
            this.IsAlive = true;
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            base.LoadContent(content, wasReloaded);
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
