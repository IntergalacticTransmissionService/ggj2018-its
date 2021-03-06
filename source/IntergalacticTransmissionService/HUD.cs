using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using IntergalacticTransmissionService.Net;
using MonoGame_Engine.Gfx;

namespace IntergalacticTransmissionService
{

    class HUD : Entity
    {
        private ITSGame game;
        private SpriteBatch spriteBatch;
        private readonly string lanIp;

        private Texture2D[] chars = new Texture2D[4];
        private Texture2D distanceScale;
        private readonly Image distanceMarker;
        private Vector2[] pos = new Vector2[4];
        private float distanceToMotherShip;

        private Texture2D[] textbox = new Texture2D[9];
        private string[] texts = new string[4];

        public float ScreenBorder = 30;

        private Sprite Title;

        public HUD(ITSGame game)
        {
            this.game = game;
            lanIp = WebControllerManager.getLanIpWithPort();

            Title = new Sprite("Images/title.png", 500, Color.White, false);
            distanceMarker = new Image("Images/distanceIndicator-marker.png");

        }


        internal override void Draw(SpriteBatch _, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            var lanIpSize = game.Fonts.Get(MonoGame_Engine.Font.DebugFont).MeasureString(lanIp);
            spriteBatch.DrawString(game.Fonts.Get(MonoGame_Engine.Font.DebugFont), lanIp, new Vector2((game.Screen.CanvasWidth - lanIpSize.X) / 2, game.Screen.CanvasHeight - 20), Color.White);

            for (int i = 0; i < 4; i++)
            {
                pos[i].X = i % 2 == 0 ? ScreenBorder : game.Screen.CanvasWidth - ScreenBorder - chars[i].Width;
                pos[i].Y = i / 2 == 0 ? ScreenBorder : game.Screen.CanvasHeight - ScreenBorder - chars[i].Height;

                if (game.MainScene.Players.Count > i)
                {
                    var player = game.MainScene.Players[i];

                    var fx = i % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    var alpha = (gameTime.TotalGameTime.TotalMilliseconds % 300 < 150) ? 0.2f : 1.0f;
                    var color = new Color(1f, 1f, 1f, player.IsInvincible ? alpha : 1f);
                    spriteBatch.Draw(chars[i], pos[i], null, color, 0, Vector2.Zero, Vector2.One, fx, 0);

                    if (!player.IsAlive)
                    {
                        var text = "You died!\r\nPress Button to join";
                        var textPosXOffset = i % 2 == 0 ? pos[i].X + chars[i].Width : pos[i].X;
                        var textPosYOffset = i / 2 == 0 ? pos[i].Y : pos[i].Y + chars[i].Height - chars[2].Height;
                        DrawTextBox(i, spriteBatch, text, new Vector2(textPosXOffset, textPosYOffset), new Color(player.BaseColor, alpha));
                    }
                    else if (player.EventTextTime > TimeSpan.Zero)
                    {
                        player.EventTextTime -= gameTime.ElapsedGameTime;
                        var textPosXOffset = i % 2 == 0 ? pos[i].X + chars[i].Width : pos[i].X;
                        var textPosYOffset = i / 2 == 0 ? pos[i].Y : pos[i].Y + chars[i].Height - chars[2].Height;
                        DrawTextBox(i, spriteBatch, player.EventText, new Vector2(textPosXOffset, textPosYOffset), player.BaseColor);
                    }
                }
            }

            const float minDistance = 2000;
            const float blendInMagic = 0.1f;
            float offset = MathHelper.Clamp((this.distanceToMotherShip - (minDistance)) , -100 / blendInMagic, 0 ) * blendInMagic;
            //if (distanceToMotherShip > minDistance)
            {
                const float preMultiplier = 0.0005f;
                const float postMultiplier = 200f;
                const float log = 5;
                const float max = 400;
                var position = (float)Math.Log(distanceToMotherShip * preMultiplier, log) * postMultiplier;
                position = MathHelper.Clamp(position, 0, max);
                var left = game.Screen.CanvasWidth / 2 - max / 2;
                var rigth = game.Screen.CanvasWidth / 2 + max / 2;
                var indicator = position + left;

                spriteBatch.Draw(distanceScale, new Vector2(left, offset));
                distanceMarker.Draw(spriteBatch, new Vector2(indicator, offset + distanceMarker.Height / 2), 0f);
            }

            if (game.MainScene.Players.Count == 0)
            {
                Title.Draw(spriteBatch, gameTime);
            }

            spriteBatch.End();
        }

        private void DrawTextBox(int charIdx, SpriteBatch spriteBatch, string text, Vector2 pos, Color color)
        {
            var fromRight = charIdx % 2 == 1;
            var size = game.Fonts.Get(MonoGame_Engine.Font.DebugFont).MeasureString(text);
            size.Y = Math.Max(size.Y, chars[0].Height - textbox[0].Height - textbox[6].Height);
            var outer = size + new Vector2(textbox[0].Width + textbox[2].Width, 0);

            if (fromRight)
                pos.X -= outer.X;

            spriteBatch.Draw(textbox[0], pos, Color.White);
            spriteBatch.Draw(textbox[1], pos + Vector2.UnitX * textbox[0].Width, null, Color.White, 0, Vector2.Zero, new Vector2(size.X / textbox[1].Width, 1), SpriteEffects.None, 0);
            spriteBatch.Draw(textbox[2], pos + Vector2.UnitX * (textbox[0].Width + size.X), Color.White);
            spriteBatch.Draw(textbox[3], pos + Vector2.UnitY * textbox[0].Height, null, Color.White, 0, Vector2.Zero, new Vector2(1, size.Y / textbox[4].Height), SpriteEffects.None, 0);
            spriteBatch.Draw(textbox[4], pos + new Vector2(textbox[0].Width, textbox[0].Height), null, Color.White, 0, Vector2.Zero, new Vector2(size.X / textbox[4].Width, size.Y / textbox[4].Height), SpriteEffects.None, 0);
            spriteBatch.Draw(textbox[5], pos + new Vector2(textbox[0].Width + size.X, textbox[0].Height), null, Color.White, 0, Vector2.Zero, new Vector2(1, size.Y / textbox[4].Height), SpriteEffects.None, 0);
            spriteBatch.Draw(textbox[6], pos + Vector2.UnitY * (textbox[0].Height + size.Y), Color.White);
            spriteBatch.Draw(textbox[7], pos + new Vector2(textbox[0].Width, textbox[0].Height + size.Y), null, Color.White, 0, Vector2.Zero, new Vector2(size.X / textbox[7].Width, 1), SpriteEffects.None, 0);
            spriteBatch.Draw(textbox[8], pos + new Vector2(textbox[0].Width + size.X, textbox[0].Height + size.Y), Color.White);
            spriteBatch.DrawString(game.Fonts.Get(MonoGame_Engine.Font.DebugFont), text, pos + new Vector2(textbox[0].Width, textbox[0].Height), color);
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            // load chars
            for (int i = 0; i < 4; ++i)
                chars[i] = content.Load<Texture2D>($"Images/character_{i + 1:00}.png");

            // load distance indicator
            distanceScale = content.Load<Texture2D>("Images/DistanceIndicator.png");
            distanceMarker.LoadContent(content, wasReloaded);

            // load textbox
            var tb = new string[] { "tl", "t", "tr", "l", "m", "r", "bl", "b", "br" };
            for (int i = 0; i < 9; ++i)
                textbox[i] = content.Load<Texture2D>($"Images/textbox_{tb[i]}.png");

            // load title
            Title.LoadContent(content);
            Title.Phy.Pos = new Vector2(game.Screen.CanvasWidth * 0.2f, game.Screen.CanvasHeight * 0.4f);
        }

        internal override void Update(GameTime gameTime)
        {
            this.distanceToMotherShip = Math.Abs((game.MainScene.Leviathan.Phy.Pos - game.Camera.Phy.Pos).Length());



            Title.Phy.Rot = -0.1f * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 0.3f);
            Title.Update(gameTime);
        }

        internal void ShowMessageForPlayer(Player player, string msg, TimeSpan duration)
        {

        }

    }
}
