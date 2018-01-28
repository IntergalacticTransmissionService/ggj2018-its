using Microsoft.Xna.Framework;
using MonoGame_Engine.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MonoGame_Engine.Phy;
using MonoGame_Engine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace IntergalacticTransmissionService.Input
{
    public class PlayfieldCamController : Entity
    {
        private readonly ITSGame game;
        private const float MinZoom = 0.65f;
        private const float MaxZoom = 1.1f;

        public PlayfieldCamController(ITSGame game, int playerIdx)
        {
            this.game = game;
        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
        }

        internal override void Update(GameTime gameTime)
        {
            var players = (game.Scenes.Current as MainScene).Players.FindAll(p=>p.IsAlive);
            if (players?.Count > 0)
            {
                var parcelWeight = players.Count() * 3;
                var centerX = (players.Average(e => e.Phy.Pos.X) + game.MainScene.Parcel.Phy.Pos.X * parcelWeight) / (float)(parcelWeight + 1);
                var centerY = (players.Average(e => e.Phy.Pos.Y) + game.MainScene.Parcel.Phy.Pos.Y * parcelWeight) / (float)(parcelWeight + 1);

                var left = Math.Min(game.MainScene.Parcel.Phy.Pos.X, players.Min(e => e.Phy.Pos.X))-100;
                var right = Math.Max(game.MainScene.Parcel.Phy.Pos.X, players.Max(e => e.Phy.Pos.X))+100;
                var top = Math.Min(game.MainScene.Parcel.Phy.Pos.Y, players.Min(e => e.Phy.Pos.Y))-75;
                var bottom = Math.Max(game.MainScene.Parcel.Phy.Pos.Y, players.Max(e => e.Phy.Pos.Y))+75;

                var distX = Math.Abs(right - left);
                var distY = Math.Abs(bottom - top);

                var zoomX = (game.Screen.CanvasWidth * 0.5f) / distX;
                var zoomY = (game.Screen.CanvasHeight * 0.5f)/ distY;
                var zoom = MathHelper.Clamp(Math.Min(zoomX, zoomY), MinZoom, MaxZoom);

                Vector2 delta = new Vector2(centerX, centerY) - game.Camera.Phy.Pos;
                game.Camera.Phy.Spd = delta * 30;

                //game.Camera.Phy.Pos.X = centerX;
                //game.Camera.Phy.Pos.Y = centerY;
                game.Camera.Zoom = zoom;
            }
        }
    }
}
