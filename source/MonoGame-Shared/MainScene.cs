using Microsoft.Xna.Framework;
using MonoGame_Engine.Engine;
using MonoGame_Shared.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Shared
{
    public class MainScene : Scene
    {
        internal readonly List<Player> Players;

        public MainScene(MonoGame game) : base(game)
        {
            Players = new List<Player>();
        }

        internal override void Initialize()
        {
            base.Initialize();
            this.BgColor = Color.Black;
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CheckForNewPlayers();
        }

        internal override int HandleInput(GameTime gameTime)
        {
            var numPlayers = base.HandleInput(gameTime);

            // register Players
            if (numPlayers < 5)
            {
                //dbgOverlay.Text = string.Format("Player {0}, please press a button", numPlayers);
                game.Inputs.AssignToPlayer(numPlayers);
            }

            return numPlayers;
        }


        private void CheckForNewPlayers()
        {
            while (Players.Count < game.Inputs.NumPlayers)
            {
                var player = new Player(game, Players.Count, 32f);
                player.LoadContent(game.Content);
                Children.Add(player);

                AccelController controller = new AccelController(game, Players.Count, player);
                controller.LoadContent(game.Content);
                Children.Add(controller);

                Players.Add(player);
            }
        }
    }
}
