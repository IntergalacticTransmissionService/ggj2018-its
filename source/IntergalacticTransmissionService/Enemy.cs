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

        public Enemy(ITSGame game, Color baseColor, float radius, Vector2 startPos) : base(game, "Images/enemy.png", baseColor, radius)
        {
            this.StartPos = startPos;
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            base.LoadContent(content, wasReloaded);
            Phy.Pos = StartPos;
        }
    }
}
