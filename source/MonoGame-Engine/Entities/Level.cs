using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntergalacticTransmissionService;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Engine.Entities
{
    public class Level : Entity
    {
        private SoundEffect sndPowerUp;

        public Level(ITSGame game, float minSpanDistance, float maxSpanDistance, int itemsCount, float speed, TimeSpan spawnRate)
        {
            if (itemsCount < 1)
                throw new ArgumentOutOfRangeException(nameof(itemsCount), itemsCount, "must 1 or greater");
            this.minSpanDistance = minSpanDistance;
            this.maxSpanDistance = maxSpanDistance;

            this.collectebelsPool = new Pool(itemsCount, 5);
            this.game = game;
            this.speed = speed;
            this.spawnRate = spawnRate;
        }


        internal readonly Pool collectebelsPool;
        private readonly ITSGame game;
        private readonly float speed;
        private readonly TimeSpan spawnRate;
        private TimeSpan lstSpan;
        private readonly float minSpanDistance;
        private readonly float maxSpanDistance;

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.collectebelsPool.Draw(spriteBatch, gameTime);
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            this.collectebelsPool.LoadContent(content, wasReloaded);
            sndPowerUp = content.Load<SoundEffect>("Sounds/powerup");
        }

        internal override void Update(GameTime gameTime)
        {
            if (this.collectebelsPool.Available > 0 && this.lstSpan.Ticks < 0)
            {
                lstSpan = spawnRate;
                var camaraWidth = this.game.Camera.TopLeft.X - this.game.Camera.TopRight.X;
                var camaraHeigt = this.game.Camera.TopLeft.Y - this.game.Camera.BottomRight.Y;

                var position = new Vector2(Math.RandomFuncs.FromRange(camaraWidth / 2, camaraWidth), Math.RandomFuncs.FromRange(camaraHeigt / 2, camaraHeigt));
                position *= new Vector2(Math.RandomFuncs.FromRange(0, 1) > 0.5 ? 1f : -1f, Math.RandomFuncs.FromRange(0, 1) > 0.5 ? 1f : -1f);
                position += this.game.Camera.Phy.Pos;

                var type = (CollectableType)Math.RandomFuncs.FromRangeInt(0, CollectableType.GetValues(typeof(CollectableType)).Length);

                var x = this.collectebelsPool.Get(type, position, 0, TimeSpan.FromSeconds(Math.RandomFuncs.FromRange(20, 30)));
                x.Phy.RotSpd = 1f;
                var direction = (this.game.Camera.Phy.Pos - position);
                direction.Normalize();
                x.Phy.Spd = direction * this.speed;
            }
            lstSpan -= gameTime.ElapsedGameTime;
            this.collectebelsPool.Update(gameTime);

        }

        internal CollectableType? Collides(Player player)
        {
            var colidedItem = this.collectebelsPool.FirstOrDefault(item => item.Phy.CollidesWith(player.Phy));
            colidedItem?.Phy.CollidesWith(player.Phy);
            var type = colidedItem?.Graphics;
            colidedItem?.Dispose();

            if (type.HasValue)
                sndPowerUp.Play();
            return type;
        }
    }
}
