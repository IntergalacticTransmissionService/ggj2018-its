using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace IntergalacticTransmissionService
{
    class Enemy : GameObject
    {
        private Vector2 StartPos;
        private float StartRot;

        public Enemy(ITSGame game, Color baseColor, float radius, Vector2 startPos, float startRot = 0) : base(game, "Images/enemy.png", baseColor, radius)
        {
            this.StartPos = startPos;
            this.StartRot = startRot;
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            base.LoadContent(content, wasReloaded);
            Phy.Pos = StartPos;
            Phy.Rot = StartRot;
        }
    }
}
