using System;
using System.Collections.Generic;
using System.Text;
using IntergalacticTransmissionService.Behaviors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace IntergalacticTransmissionService
{
    class Leviathan : GameObject
    {
        public Vector2 StartPos;
        public float StartRot;

        public Behavior Behavior { get; set; }
        public bool IsAlive { get; internal set; }

        private SoundEffect sndExplode;

        public Leviathan(ITSGame game, Color baseColor, float radius, Vector2 startPos, float startRot = 0, Behavior behavior = null) : base(game, "Images/Leviathan.png", baseColor, radius,false)
        {
            this.StartPos = startPos;
            this.StartRot = startRot;
            this.Behavior = behavior;
            this.IsAlive = true;
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
            //Die(); The leviathan downt die :)
        }
    }
}
