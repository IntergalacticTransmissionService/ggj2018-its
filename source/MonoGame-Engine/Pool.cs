using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGame_Engine
{
    class Pool<T> : Entities.Entity, IEnumerable<T>
        where T : Entities.Entity
    {
        private readonly Queue<T> queue;
        private readonly PoolToSmallbehavior smallbehavior;

        private readonly Func<T> generator;

        public Pool(int initialSize, PoolToSmallbehavior smallbehavior, Func<T> generator)
        {
            this.queue = new Queue<T>(Enumerable.Range(0, initialSize).Select(x => generator()));
            this.smallbehavior = smallbehavior;
            this.generator = generator;
        }

        internal void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            foreach (var item in queue)
                item.LoadContent(content, wasReloaded);
        }

        // Pool will manage everything. (Nix entfernen)
        //public T GetElement()
        //{
        //    if (this.queue.Count == 0)
        //    {
        //        switch (this.smallbehavior)
        //        {
        //            case PoolToSmallbehavior.ReturnDefault:
        //                return default;
        //            case PoolToSmallbehavior.ThrowException:
        //                throw new InvalidOperationException("Pool Empty. (Check your game code)");
        //            case PoolToSmallbehavior.AddElements:
        //                this.queue.Enqueue(this.generator());
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    }
        //    return this.queue.Dequeue();
        //}

        public void ReturnElement(T element) => this.queue.Enqueue(element);
    }



    enum PoolToSmallbehavior
    {
        ReturnDefault,
        ThrowException,
        AddElements
    }
}
