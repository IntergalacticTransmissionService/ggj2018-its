using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using IntergalacticTransmissionService.Net;

namespace IntergalacticTransmissionService
{
    class HUD : Entity
    {
        private ITSGame game;
        private SpriteBatch spriteBatch;
        private readonly string lanIp;
        public Vector2 lanIpPos = new Vector2();

        private Texture2D[] chars = new Texture2D[4];
        private Vector2[] pos = new Vector2[4];

        public HUD(ITSGame game)
        {
            this.game = game;
            lanIp = WebControllerManager.getLanIpWithPort();
        }

        internal override void Draw(SpriteBatch _, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(game.Fonts.Get(MonoGame_Engine.Font.DebugFont), lanIp, lanIpPos, Color.White);

            for(int i=0; i<4; i++)
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
                        var textPosXOffset = i % 2 == 0 ? 0 : -chars[i].Width-game.Fonts.Get(MonoGame_Engine.Font.DebugFont).MeasureString(text).Y;
                        var textPosYOffset = i / 2 == 0 ? chars[i].Height + 10 : -chars[i].Height - 10;

                        spriteBatch.DrawString(game.Fonts.Get(MonoGame_Engine.Font.DebugFont), text, new Vector2(pos[i].X + textPosXOffset, pos[i].Y + textPosYOffset), new Color(player.BaseColor, alpha));
                    }
                }
            }
            spriteBatch.End();
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            for (int i = 0; i < 4; ++i)
                chars[i] = content.Load<Texture2D>($"Images/character_{i+1:00}.png");
            lanIpPos = new Vector2(chars[0].Width + 20, 10);
        }

        internal override void Update(GameTime gameTime)
        {

        }
    }
}
