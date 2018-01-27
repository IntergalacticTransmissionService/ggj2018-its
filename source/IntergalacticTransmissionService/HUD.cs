using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntergalacticTransmissionService
{

    class HUD : Entity
    {
        private ITSGame game;
        private SpriteBatch spriteBatch;

        private Texture2D[] chars = new Texture2D[4];
        private Texture2D distanceScale;
        private Vector2[] pos = new Vector2[4];
        private float distanceToMotherShip;

        public HUD(ITSGame game)
        {
            this.game = game;
        }

        internal override void Draw(SpriteBatch _, GameTime gameTime)
        {
            spriteBatch.Begin();
            for (int i = 0; i < 4; i++)
            {
                pos[i].X = i % 2 == 0 ? 10 : game.Screen.CanvasWidth - 10 - chars[i].Width;
                pos[i].Y = i / 2 == 0 ? 10 : game.Screen.CanvasHeight - 10 - chars[i].Height;

                if (game.MainScene.Players.Count > i)
                {
                    var player = game.MainScene.Players[i];
                    var fx = i % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    var alpha = (gameTime.TotalGameTime.TotalMilliseconds % 300 < 150) ? 0.2f : 1.0f;
                    var color = new Color(1f, 1f, 1f, player.IsInvincible ? alpha : 1f);
                    spriteBatch.Draw(chars[i], pos[i], null, color, 0, Vector2.Zero, Vector2.One, fx, 0);

                    if (!player.IsAlive)
                    {
                        var text = "Press Button to join";
                        var textPosXOffset = i % 2 == 0 ? 0 : -chars[i].Width - game.Fonts.Get(MonoGame_Engine.Font.DebugFont).MeasureString(text).Y;
                        var textPosYOffset = i / 2 == 0 ? chars[i].Height + 10 : -chars[i].Height - 10;

                        spriteBatch.DrawString(game.Fonts.Get(MonoGame_Engine.Font.DebugFont), text, new Vector2(pos[i].X + textPosXOffset, pos[i].Y + textPosYOffset), new Color(player.BaseColor, alpha));
                    }
                }
            }
            if (distanceToMotherShip > 1000)
            {
                var position = (float)Math.Log(distanceToMotherShip,1.1);
                position = MathHelper.Clamp(position, 0, 400);
                var left = game.Screen.CanvasWidth / 2 - 200;
                var rigth = game.Screen.CanvasWidth / 2 + 200;
                var indicator = position + left;

                spriteBatch.Draw(distanceScale, new Vector2(left, 0));
                this.game.MainScene.leviathan.Gfx.Draw(spriteBatch, new Vector2(indicator, 10), MathHelper.PiOver2, 10f, Color.White);
            }
            spriteBatch.End();
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            for (int i = 0; i < 4; ++i)
                chars[i] = content.Load<Texture2D>($"Images/character_{i + 1:00}.png");
            distanceScale = content.Load<Texture2D>("Images/DistanceIndicator.png");
        }

        internal override void Update(GameTime gameTime)
        {
            this.distanceToMotherShip = Math.Abs((game.MainScene.leviathan.Phy.Pos - game.Camera.Phy.Pos).Length());
        }
    }
}
