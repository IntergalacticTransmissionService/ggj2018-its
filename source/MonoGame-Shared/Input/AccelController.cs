using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame_Engine.Entities;
using MonoGame_Engine;
using MonoGame_Engine.Input;

namespace MonoGame_Shared.Input
{
    public class AccelController : Entity
    {
        private readonly BaseGame game;
        private int playerIdx;

        private Texture2D tex;
        private Vector2 origin;
        private Vector2 scale;
        private Color color;

        public Player Player { get; set; }

        public AccelController(BaseGame game, int playerIdx, Player player)
        {
            this.game = game;
            this.playerIdx = playerIdx;
            this.Player = player;
            Player.Phy.Dmp = 0.95f;
        }

        internal override void LoadContent(ContentManager content, bool wasReloaded = false)
        {
            tex = content.Load<Texture2D>("Images/particle.png");
            if (!wasReloaded)
            {
                origin = new Vector2(tex.Width * 0.5f, tex.Height * 0.5f);
                scale = new Vector2(0.7f, 0.7f);
                color = new Color(0.2f, 0.2f, 1.0f, 1.0f);
            }
        }

        internal override void Update(GameTime gameTime)
        {
            if (Player != null && Player.Phy != null && game.Inputs.NumPlayers > playerIdx)
            {
                var cntrl = game.Inputs.Player(playerIdx);

                // apply movement
                var mat = Matrix.CreateRotationZ(game.Camera.Phy.Rot);
                var movement = Vector3.Transform(
                    new Vector3(cntrl.Value(Sliders.LeftStickX), cntrl.Value(Sliders.LeftStickY), 0) * 4000,
                    mat);

                Player.Phy.Accel.X += movement.X;
                Player.Phy.Accel.Y += movement.Y;

                // rumble
                if (cntrl.WasPressed(Buttons.B))
                    Player.WasHit();
            }
        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var pos = new Vector2(Player.Phy.Pos.X, Player.Phy.Pos.Y);
            for (int i = 1; i < 10; ++i)
            {
                spriteBatch.Draw(tex, pos, null, null, origin, 0, scale, Player.BaseColor);
                pos.X += Player.Phy.Spd.X * i * 0.01f;
                pos.Y += Player.Phy.Spd.Y * i * 0.01f;
            }
        }
    }
}
