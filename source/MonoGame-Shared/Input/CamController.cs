using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Engine;
using MonoGame_Engine.Engine.Input;
using MonoGame_Engine.Math.Funcs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Shared.Input
{
    public class CamController : Entity
    {
        private readonly BaseGame game;
        private readonly int playerIdx;

        public CamController(BaseGame game, int playerIdx)
        {
            this.game = game;
            this.playerIdx = playerIdx;
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

            // Zoom
            amount = cntrl.Value(Sliders.RightStickY);
            game.Camera.Zoom = 1 / (amount + 1.4f);
        }
    }
}
