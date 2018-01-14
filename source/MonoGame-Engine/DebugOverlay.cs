using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Engine
{
    public class DebugOverlay
    {
        private readonly BaseGame game;
        private SpriteBatch spriteBatch;

        public string Text = "";

        public DebugOverlay(BaseGame game)
        {
            this.game = game;
        }

        internal void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(game.Fonts.Get(Font.DebugFont), Text, Vector2.Zero, Color.Red);
            spriteBatch.End();
        }
    }
}
