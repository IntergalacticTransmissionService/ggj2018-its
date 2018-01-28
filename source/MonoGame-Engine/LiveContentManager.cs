using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace MonoGame_Engine
{
    class AssetLookup
    {
        private const string FILENAME = "/assetmap.ini";

#if DEBUG
        private readonly FileSystemWatcher watcher;
#endif    
        private readonly LiveContentManager content;
        public readonly Dictionary<string, string> Map;

        private bool changed;
        public bool Changed
        {
            get { return changed; }
            private set
            {
                changed = value;
#if DEBUG
                watcher.EnableRaisingEvents = !changed;
#endif            
            }
        }

        public AssetLookup(LiveContentManager content)
        {
            this.content = content;
            this.Map = new Dictionary<string, string>();
#if DEBUG
            this.watcher = new FileSystemWatcher();
            RegisterWatcher();
#endif
            Load();
        }

#if DEBUG
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private void RegisterWatcher()
        {
            var mapFile = content.LiveContentPath + FILENAME;
            if (File.Exists(mapFile))
            {
                watcher.Path = Path.GetDirectoryName(mapFile);
                watcher.Filter = Path.GetFileName(mapFile);
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                watcher.EnableRaisingEvents = true;
                watcher.Changed += (sender, args) =>
                {
                    Changed = true;
                };
            }
        }
#endif

        private void ReadFile(string mapFile)
        {
            string[] lines = File.ReadAllLines(mapFile);
            Map.Clear();
            foreach (var line in lines)
            {
                if (!line.StartsWith("#") && !line.StartsWith(";") && !string.IsNullOrWhiteSpace(line))
                {
                    var tokens = line.Split('=');
                    var key = tokens[0];
                    var value = tokens[1];
#if DEBUG
                    if (File.Exists(content.LiveContentPath + "/" + value))
#else
                    value = Path.ChangeExtension(value, "xnb");
                    if (File.Exists(content.LiveContentPath + "/" + value))
#endif
                    {
                        Map.Add(key, value);
                        Console.Error.WriteLine($"Added asset replacement: {key} => {value}");
                    }
                    else
                        Console.Error.WriteLine($"Error in AssetMap: {value} not found as replacement for {key}");
                }
            }
        }

        private void Load()
        {
            var mapFile = content.LiveContentPath + FILENAME;
            if (File.Exists(mapFile))
            {
                var maxTries = 3;
                do
                {
                    try
                    {
                        ReadFile(mapFile);
                        break;
                    }
                    catch (IOException)
                    {
                        Console.Error.WriteLine($"File {mapFile} is locked. Trying again...");
                        --maxTries;
                        Thread.Sleep(1000);
                    }
                } while (maxTries > 0);
            }
            else
            {
                Console.Error.WriteLine($"AssetMap not found at {mapFile}");
            }
            Changed = false;
        }

        public void Reload()
        {
#if DEBUG
            Load();
#endif
        }
    }

    public class LiveContentManager : ContentManager
    {
        private readonly BaseGame game;

        private readonly AssetLookup AssetLookup;

        public string LiveContentPath
        {
            get
            {
#if DEBUG
                if (Directory.Exists("../../../../../IntergalacticTransmissionService-Content/Content"))
                    return "../../../../../IntergalacticTransmissionService-Content/Content";
                return RootDirectory;
#else
                return RootDirectory;
#endif
            }
        }

        public LiveContentManager(BaseGame game) : base(game.Services)
        {
            this.game = game;
            this.RootDirectory = "Content";
            this.AssetLookup = new AssetLookup(this);
        }

        public void Update(GameTime gameTime)
        {
            if (AssetLookup.Changed)
                ReloadAll();
        }

        public override T Load<T>(string asset)
        {
            string path;

            if (AssetLookup.Map.ContainsKey(asset))
                asset = AssetLookup.Map[asset];

#if DEBUG
            if (typeof(T) == typeof(Texture2D))
            {
                if (asset.Contains(".jpg") || asset.Contains(".png"))
                {
                    path = this.LiveContentPath + "/" + asset;
                    using (var stream = new FileStream(path, FileMode.Open))
                        return (T)Convert.ChangeType(Texture2D.FromStream(game.GraphicsDevice, stream), typeof(T));
                }
            }
#endif

            asset = $"{Path.GetDirectoryName(asset)}/{Path.GetFileNameWithoutExtension(asset)}";
            path = base.RootDirectory + "/" + asset + ".xnb";
            if (!File.Exists(path))
            {
                Console.Error.WriteLine($"Could not find file {path}");
            }
            return base.Load<T>(asset);
        }

        public void ReloadAll()
        {
            AssetLookup.Reload();
            Reloadable.ReloadAll(this.game);
        }
    }
}
