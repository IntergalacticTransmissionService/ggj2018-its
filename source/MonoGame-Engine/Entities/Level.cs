using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Engine.Entities
{
    class Level : Entity
    {

        public Level(float minSpanDistance, float maxSpanDistance, float fillRatio, int sampleRate)
        {
            if (sampleRate < 1)
                throw new ArgumentOutOfRangeException(nameof(sampleRate), sampleRate, "must 1 or greater");
            this.minSpanDistance = minSpanDistance;
            this.maxSpanDistance = maxSpanDistance;
            this.fillRatio = fillRatio;
            this.sampleRate = sampleRate;
        }

        public Vector2 Center { get; set; }

        private readonly Pool<LevelEntety> collectebelsPool = new Pool<LevelEntety>(40, PoolToSmallbehavior.ReturnDefault, () => new LevelEntety("collectable.png"));
        private readonly List<LevelEntety> entrys = new List<LevelEntety>();
        private readonly float minSpanDistance;
        private readonly float maxSpanDistance;
        private readonly float fillRatio;
        private readonly int sampleRate;

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var item in this.entrys)
                item.Draw(spriteBatch, gameTime);
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            this.collectebelsPool.LoadContent(content, wasReloaded);
            foreach (var item in this.entrys)
                item.LoadContent(content, wasReloaded);
        }

        internal override void Update(GameTime gameTime)
        {

            var itemsToDelete = new List<LevelEntety>();



            var spanVector = new Vector2(Math.RandomFuncs.FromRange(this.minSpanDistance, this.maxSpanDistance), Math.RandomFuncs.FromRange(this.minSpanDistance, this.maxSpanDistance));
            var delta = this.maxSpanDistance - this.minSpanDistance;

            var blocksize = delta / this.sampleRate;


            var sectors = this.entrys.GroupBy(x =>
            {
                var relativPosition = x.Phy.Pos - Center;
                ////to near to be interesting
                //if (System.Math.Abs(relativPosition.Y) < minSpanDistance && System.Math.Abs(relativPosition.X) < minSpanDistance)
                //    return (float.PositiveInfinity, float.PositiveInfinity);

                // To far away, delete
                if (System.Math.Abs(relativPosition.Y) > maxSpanDistance || System.Math.Abs(relativPosition.X) > maxSpanDistance)
                    return (float.NaN, float.NaN);

                var sector = relativPosition / blocksize;

                return (sector.X, sector.Y);
            }).ToArray();


            foreach (var item in this.entrys)
                item.Update(gameTime);




        }
    }
}
