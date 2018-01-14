﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Engine.Components;
using Microsoft.Xna.Framework.Content;
using MonoGame_Engine.Components;

namespace MonoGame_Engine.Engine.Entities
{
    public class Sprite : Entity, IHasPhysics
    {
        public Physics Phy { get; private set; }
        public Image Gfx { get; private set; }

        public Sprite(Image img)
        {
            Gfx = img;
        }

        public Sprite(string assetPath)
        {
            Gfx = new Image(assetPath);
        }

        public void LoadContent(ContentManager content)
        {
            Gfx.LoadContent(content);
            Phy = new Physics(Gfx.origin.X);
        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Gfx.Draw(spriteBatch, Phy.Pos, Phy.Rot);
        }

        internal override void Update(GameTime gameTime)
        {
            Phy.Update(gameTime);
        }
    }
}
