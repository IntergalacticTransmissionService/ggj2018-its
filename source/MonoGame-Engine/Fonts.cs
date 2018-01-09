using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MonoGame_Engine.Engine
{
    public class Fonts
    {
        private readonly BaseGame game;
        private readonly Dictionary<Font, SpriteFont> fonts;

        public Fonts(BaseGame game)
        {
            this.game = game;
            this.fonts = new Dictionary<Font, SpriteFont>();
        }

        public void LoadFonts()
        {
            foreach (Font font in Enum.GetValues(typeof(Font)))
            {
                var spriteFont = game.Content.Load<SpriteFont>($"Fonts/{font.ToString()}");
                fonts.Add(font, spriteFont);
            }
        }


        public SpriteFont Get(Font name)
        {
            return fonts[name];
        }

        public void Clear()
        {
            fonts.Clear();
        }
    }

    public enum Font
    {
        DebugFont
    }
}

