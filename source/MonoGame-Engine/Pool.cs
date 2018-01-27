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
    public enum CollectableType : int
    {
        RapidFire,
        SpreadShoot,
        BackShoot,
        UpDownShoot,
    }
    class Pool : Entities.Entity, IEnumerable<ICollectable>
    {
        private readonly Collectable[] list;
        private readonly Collectable[] backupList;
        private int lastActive = -1;

        public int Count => this.lastActive + 1;
        public int Available => this.list.Length - this.Count;

        public Pool(int initialSize, float itemRadius)
        {
            this.list = new Collectable[initialSize];
            this.backupList = new Collectable[initialSize];
            for (int i = 0; i < this.list.Length; i++)
                this.list[i] = new Collectable(i, this) { Phy = new Phy.Physics(itemRadius) };
        }
        internal override void Update(GameTime gameTime)
        {
            foreach (var toUpdate in this.OfType<Collectable>())
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
            foreach (var toDraw in this.list.Take(this.Count)) {
                var scale = 1.0f;
                if(toDraw.timeToLive < TimeSpan.FromSeconds(5.0))
                    scale = (float)(toDraw.timeToLive.TotalSeconds / 5.0);
                scale *= 1.4f;
                this.Graphics[(int)toDraw.Graphics].Draw(spriteBatch, toDraw.Phy.Pos, toDraw.Phy.Rot, toDraw.Phy.HitBox.Radius * scale, Color.White);
            }
        }

        internal virtual IReadOnlyList<Gfx.Image> Graphics { get; } = new Gfx.Image[] { new Gfx.Image("Images/collectableRapidFire.png"), new Gfx.Image("Images/collectableSpread.png"), new Gfx.Image("Images/collectableBack.png"), new Gfx.Image("Images/collectableUpDown.png"), };

        public ICollectable Get(CollectableType grafic, Vector2 position, float rotation, TimeSpan timeToLive)
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
            newCollectibal.Graphics = grafic;
            newCollectibal.timeToLive = timeToLive;
            return newCollectibal;
        }

        protected void ReturnCollectible(Collectable c)
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

        public IEnumerator<ICollectable> GetEnumerator()
        {
            // save for eventuell deletes while itterating
            //Array.Copy(this.list, this.backupList, this.list.Length);
            //return this.backupList.Take(this.Count).GetEnumerator();
            return this.list.Take(this.Count).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();


        protected class Collectable : ICollectable
        {
            public int index;
            private Pool pool;

            public TimeSpan timeToLive;

            public CollectableType Graphics { get; set; }
            public Phy.Physics Phy { get; set; }

            public bool IsActive => this.index <= this.pool.lastActive;

            public Collectable(int i, Pool pool)
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
