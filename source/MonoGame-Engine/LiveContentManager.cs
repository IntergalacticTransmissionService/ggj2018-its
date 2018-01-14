using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonoGame_Engine
{
    public class LiveContentManager : ContentManager
    {
        private readonly BaseGame game;
        public readonly string LiveContentPath;

        public LiveContentManager(BaseGame game) : this(game, ((Game)game).Content.RootDirectory)
        {
            
        }

        public LiveContentManager(BaseGame game, string liveContentPath) : base(game.Services)
        {
            this.game = game;
            this.LiveContentPath = liveContentPath;
        }

        public override T Load<T>(string asset)
        {
            if (typeof(T) == typeof(Texture2D))
            {
#if DEBUG
                if (asset.Contains(".jpg") || asset.Contains(".png"))
                {
                    var path = this.LiveContentPath + "/" + asset;
                    using (var stream = new FileStream(path, FileMode.Open))
                        return (T)Convert.ChangeType(Texture2D.FromStream(game.GraphicsDevice, stream), typeof(T));
                }
#endif
                asset = $"{Path.GetDirectoryName(asset)}/{Path.GetFileNameWithoutExtension(asset)}";
            }
            return base.Load<T>(asset);
        }

        public void ReloadAll()
        {
            Reloadable.ReloadAll(this.game);
        }
    }
}
