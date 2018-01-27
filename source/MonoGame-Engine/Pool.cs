using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGame_Engine
{
    public enum CollectibleType : int
    {
        Stuff = 0,
    }
    class Pool : Entities.Entity, IEnumerable<ICollecteble>
    {
        private readonly Collecteble[] list;
        private readonly Collecteble[] backupList;
        private int lastActive = -1;

        public int Count => this.lastActive + 1;
        public int Available => this.list.Length - this.Count;

        public Pool(int initialSize, float itemRadios)
        {
            this.list = new Collecteble[initialSize];
            this.backupList = new Collecteble[initialSize];
            for (int i = 0; i < this.list.Length; i++)
                this.list[i] = new Collecteble(i, this) { Phy = new Phy.Physics(itemRadios) };
        }
        internal override void Update(GameTime gameTime)
        {
            foreach (var toUpdate in this.OfType<Collecteble>())
            {
                toUpdate.Phy.Update(gameTime);
                toUpdate.timeToLive -= gameTime.ElapsedGameTime;
                if (toUpdate.timeToLive.Ticks < 0)
                    toUpdate.Dispose();
            }
        }
        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            foreach (var item in this.Graphics)
                item.LoadContent(content, wasReloaded);
        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var toDraw in this.list.Take(this.Count).Where(Blink))
                this.Graphics[(int)toDraw.Grafic].Draw(spriteBatch, toDraw.Phy.Pos, toDraw.Phy.Rot);
        }

        private bool Blink(Collecteble arg)
        {
            if (
                arg.timeToLive > TimeSpan.FromSeconds(3.0)
                || arg.timeToLive > TimeSpan.FromSeconds(2.6) && arg.timeToLive < TimeSpan.FromSeconds(3.0)
                || arg.timeToLive > TimeSpan.FromSeconds(2.0) && arg.timeToLive < TimeSpan.FromSeconds(2.4)
                || arg.timeToLive > TimeSpan.FromSeconds(1.8) && arg.timeToLive < TimeSpan.FromSeconds(2.0)
                || arg.timeToLive > TimeSpan.FromSeconds(1.4) && arg.timeToLive < TimeSpan.FromSeconds(1.6)
                || arg.timeToLive > TimeSpan.FromSeconds(1.0) && arg.timeToLive < TimeSpan.FromSeconds(1.2)
                || arg.timeToLive > TimeSpan.FromSeconds(0.6) && arg.timeToLive < TimeSpan.FromSeconds(0.8)
                || arg.timeToLive > TimeSpan.FromSeconds(0.2) && arg.timeToLive < TimeSpan.FromSeconds(0.4)
                )
                return true;

            return false;
        }

        protected virtual IReadOnlyList<Gfx.Image> Graphics { get; } = new Gfx.Image[] { new Gfx.Image("Images/collectable.png") };

        public ICollecteble Get(CollectibleType grafic, Vector2 position, float rotation, TimeSpan timeToLive)
        {
            if (this.lastActive == this.list.Length - 1)
                return null;

            var newCollectibal = this.list[this.lastActive + 1];
            this.lastActive++;
            newCollectibal.Phy.Accel = Vector2.Zero;
            newCollectibal.Phy.Dmp = 1;
            newCollectibal.Phy.RotSpd = 0;
            newCollectibal.Phy.Spd = Vector2.Zero;
            newCollectibal.Phy.Pos = position;
            newCollectibal.Phy.Rot = rotation;
            newCollectibal.Grafic = grafic;
            newCollectibal.timeToLive = timeToLive;
            return newCollectibal;
        }

        protected void ReturnCollectible(Collecteble c)
        {
            if (c.index != this.lastActive)
            {
                // swap with last active
                var currentIndex = c.index;

                var temp = this.list[this.lastActive];
                this.list[this.lastActive] = this.list[currentIndex];
                this.list[currentIndex] = temp;
                this.list[this.lastActive].index = this.lastActive;
                this.list[currentIndex].index = currentIndex;

            }
            this.lastActive--;
        }

        public IEnumerator<ICollecteble> GetEnumerator()
        {
            // save for eventuell deletes while itterating
            //Array.Copy(this.list, this.backupList, this.list.Length);
            //return this.backupList.Take(this.Count).GetEnumerator();
            return this.list.Take(this.Count).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();


        protected class Collecteble : ICollecteble
        {
            public int index;
            private Pool pool;

            public TimeSpan timeToLive;

            public CollectibleType Grafic { get; set; }
            public Phy.Physics Phy { get; set; }

            public bool IsActive => this.index <= this.pool.lastActive;

            public Collecteble(int i, Pool pool)
            {
                this.index = i;
                this.pool = pool;
            }

            public void Dispose()
            {
                if (this.IsActive)
                    this.pool.ReturnCollectible(this);
            }
        }

    }



    enum PoolToSmallbehavior
    {
        ReturnDefault,
        ThrowException,
        AddElements
    }
}
