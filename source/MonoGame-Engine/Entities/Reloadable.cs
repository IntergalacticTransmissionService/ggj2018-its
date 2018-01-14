using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Engine.Entities
{
    public abstract class Reloadable
    {
        private static HashSet<Reloadable> reloadables = new HashSet<Reloadable>();

        public Reloadable()
        {
            reloadables.Add(this);
        }

        internal static void ReloadAll(BaseGame game)
        {
            foreach (var reloadable in reloadables)
                reloadable.LoadContent(game.Content, true);
        }

        internal abstract void LoadContent(ContentManager content, bool wasReloaded = false);
    }
}
