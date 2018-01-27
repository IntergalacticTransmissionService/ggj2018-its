using System;
using System.Collections.Generic;
using System.Text;
using IntergalacticTransmissionService.Behaviors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace IntergalacticTransmissionService
{
    class Enemy : GameObject
    {
        private Vector2 StartPos;
        private float StartRot;

        public Behavior Behavior { get; set; }

        public Enemy(ITSGame game, Color baseColor, float radius, Vector2 startPos, float startRot = 0, Behavior behavior = null) : base(game, "Images/enemy.png", baseColor, radius)
        {
            this.StartPos = startPos;
            this.StartRot = startRot;
            this.Behavior = behavior;
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            base.LoadContent(content, wasReloaded);
            Phy.Pos = StartPos;
            Phy.Rot = StartRot;
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Behavior?.Update(this, gameTime);
        }
    }
}
