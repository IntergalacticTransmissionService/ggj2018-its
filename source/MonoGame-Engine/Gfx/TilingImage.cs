using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Engine.Gfx
{
    public class TilingImage : Image
    {
        private readonly BaseGame game;

        public TilingImage(string assetPath, BaseGame game) : base(assetPath)
        {
            this.game = game;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 pos, float rot)
        {
            var oldSamplerState = spriteBatch.GraphicsDevice.SamplerStates[0];

            spriteBatch.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            var rect = MathExtensions.FromVectors(game.Camera.TopLeft, game.Camera.TopRight, game.Camera.BottomLeft, game.Camera.BottomRight);
            var topLeft = new Vector2(rect.X, rect.Y);

            spriteBatch.Draw(tex, pos+topLeft, rect, Color.White, rot, origin, Vector2.One, SpriteEffects.None, 0f);

            spriteBatch.GraphicsDevice.SamplerStates[0] = oldSamplerState;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 pos, float rot, float size, Color color)
        {
            Draw(spriteBatch, pos, rot);
        }

    }
}
