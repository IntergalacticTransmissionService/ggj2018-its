using Microsoft.Xna.Framework;
using MonoGame_Engine.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MonoGame_Engine.Phy;
using MonoGame_Engine;

namespace IntergalacticTransmissionService.Input
{
    public class PlayfieldCamController : CamController
    {
        public PlayfieldCamController(ITSGame game, int playerIdx) : base(game, playerIdx)
        {
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var players = (game.Scenes.Current as MainScene).Players;
            if (players?.Count > 1)
            {
               var centerX = players.Average(e => e.Phy.Pos.X);
               var centerY = players.Average(e => e.Phy.Pos.Y);

                var left = players.Min(e => e.Phy.Pos.X);
                var right = players.Max(e => e.Phy.Pos.X);
                var top = players.Min(e => e.Phy.Pos.Y);
                var bottom = players.Max(e => e.Phy.Pos.Y);

                var distX = Math.Abs(right - left);
                var distY = Math.Abs(bottom - top);

                var zoomX = game.Screen.Width / distX;
                var zoomY = game.Screen.Height / distY;
                var zoom = MathHelper.Clamp(Math.Min(zoomX, zoomY), 0.1f, 3f);

                game.Camera.Phy.Pos.X = centerX;
                game.Camera.Phy.Pos.Y = centerY;
                game.Camera.Zoom = zoom;
            }
        }
    }
}
