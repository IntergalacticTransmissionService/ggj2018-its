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

        public Level(ITSGame game, float minSpanDistance, float maxSpanDistance, int itemsCount, float speed)
        {
            if (itemsCount < 1)
                throw new ArgumentOutOfRangeException(nameof(itemsCount), itemsCount, "must 1 or greater");
            this.minSpanDistance = minSpanDistance;
            this.maxSpanDistance = maxSpanDistance;

            this.collectebelsPool = new Pool(itemsCount, 5);
            this.game = game;
            this.speed = speed;
        }


        internal readonly Pool collectebelsPool;
        private readonly ITSGame game;
        private readonly float speed;
        private readonly float minSpanDistance;
        private readonly float maxSpanDistance;

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
            if (this.collectebelsPool.Available > 0)
            {
                var position = new Vector2(Math.RandomFuncs.FromRange(this.game.Camera.TopRight.X, this.game.Camera.TopRight.X * 2), Math.RandomFuncs.FromRange(this.game.Camera.TopRight.Y, this.game.Camera.TopRight.Y * 2));
                position *= new Vector2(Math.RandomFuncs.FromRange(0, 1) > 0.5 ? 1f : -1f, Math.RandomFuncs.FromRange(0, 1) > 0.5 ? 1f : -1f);
                position += this.game.Camera.Phy.Pos;

                var type = (CollectableType)Math.RandomFuncs.FromRangeInt(0, CollectableType.GetValues(typeof(CollectableType)).Length);

                var x = this.collectebelsPool.Get(type, position, 0, TimeSpan.FromSeconds(Math.RandomFuncs.FromRange(20, 30)));
                x.Phy.RotSpd = 1f;
                var direction = (this.game.Camera.Phy.Pos - position);
                direction.Normalize();
                x.Phy.Spd = direction * this.speed;
            }

            this.collectebelsPool.Update(gameTime);

        }

        internal CollectableType? Collides(Player player)
        {
            var colidedItem = this.collectebelsPool.FirstOrDefault(item => item.Phy.CollidesWith(player.Phy));
            colidedItem?.Phy.CollidesWith(player.Phy);
            var type = colidedItem?.Graphics;
            colidedItem?.Dispose();
            return type;
        }
    }
}
