using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine;
using MonoGame_Engine.Entities;
using MonoGame_Engine.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Shared.Input
{
    public class CamController : Entity
    {
        protected readonly MonoGame game;
        private readonly int playerIdx;

        public CamController(MonoGame game, int playerIdx)
        {
            this.game = game;
            this.playerIdx = playerIdx;
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded)
        {

        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        internal override void Update(GameTime gameTime)
        {
            var cntrl = game.Inputs.Player(playerIdx);
            float amount;

            // rotation
            if (cntrl.IsDown(Buttons.RightStick))
            {
                game.Camera.Phy.RotSpd = -game.Camera.Phy.Rot;
            }
            else
            {
                amount = cntrl.Value(Sliders.RightStickX);
                game.Camera.Phy.RotSpd = amount;
            }

            // Spd
            Vector2 delta = (game.Scenes.Current as MainScene).Players[0].Phy.Pos - game.Camera.Phy.Pos;
            game.Camera.Phy.Spd = delta * 5;

            // Zoom
            amount = cntrl.Value(Sliders.RightStickY);
            game.Camera.Zoom = 1 / (amount + 1.4f);
        }
    }
}
