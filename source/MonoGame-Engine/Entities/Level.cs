using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntergalacticTransmissionService;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Engine.Entities
{
    public class Level : Entity
    {

        public Level(ITSGame game, float minSpanDistance, float maxSpanDistance, int itemsCount)
        {
            if (itemsCount < 1)
                throw new ArgumentOutOfRangeException(nameof(itemsCount), itemsCount, "must 1 or greater");
            this.minSpanDistance = minSpanDistance;
            this.maxSpanDistance = maxSpanDistance;

            this.collectebelsPool = new Pool(itemsCount, 5);
            this.game = game;
        }


        private readonly Pool collectebelsPool;
        private readonly ITSGame game;
        private readonly float minSpanDistance;
        private readonly float maxSpanDistance;
        private readonly float fillRatio;
        private readonly int sampleRate;

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.collectebelsPool.Draw(spriteBatch, gameTime);
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            this.collectebelsPool.LoadContent(content, wasReloaded);
        }

        internal override void Update(GameTime gameTime)
        {

            this.collectebelsPool.Update(gameTime);


            foreach (var item in this.collectebelsPool)
            {
                var relativPosition = item.Phy.Pos - game.Camera.Phy.Pos;
                if (System.Math.Abs(relativPosition.X) > this.maxSpanDistance || System.Math.Abs(relativPosition.Y) > this.maxSpanDistance)
                    item.Dispose();
            }

            while (this.collectebelsPool.Available > 0)
            {
                var position = new Vector2(Math.RandomFuncs.FromRange(this.minSpanDistance, this.maxSpanDistance), Math.RandomFuncs.FromRange(this.minSpanDistance, this.maxSpanDistance)) + game.Camera.Phy.Pos;
                var x = this.collectebelsPool.Get(CollectebleGrafic.Stuff, position, 0);
                x.Phy.RotSpd = 1f;
                var direction = (game.Camera.Phy.Pos - position);
                direction.Normalize();
                x.Phy.Spd = direction * 30f; ;
            }


        }
    }
}
