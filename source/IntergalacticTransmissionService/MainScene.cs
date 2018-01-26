﻿using IntergalacticTransmissionService.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine;
using MonoGame_Engine.Entities;
using MonoGame_Engine.Gfx;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntergalacticTransmissionService
{
    public class MainScene : Scene
    {
        internal readonly TilingImage BackgroundImg;
        internal readonly Sprite Background;

        internal readonly List<Player> Players;

        internal new ITSGame game {  get { return base.game as ITSGame; } }

        public MainScene(ITSGame game) : base(game)
        {
            BackgroundImg = new TilingImage("Images/starfield.png", game);
            Background = new Sprite(BackgroundImg);
            Players = new List<Player>();
        }

        internal override void Initialize()
        {
            base.Initialize();
            this.BgColor = Color.Black;
        }

        internal override void LoadContent()
        {
            base.LoadContent();
            Background.LoadContent(game.Content);
            Background.Gfx.origin = Vector2.Zero;
            Children.Add(Background);
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
            if (numPlayers < game.Inputs.NumPlayersAvailable)
                game.Inputs.AssignToPlayer(numPlayers);

            return numPlayers;
        }


        private void CheckForNewPlayers()
        {
            while (Players.Count < game.Inputs.NumPlayers)
            {
                var player = new Player(game, Players.Count, 32f);
                player.LoadContent(game.Content);
                Children.Add(player);

                var controller = new AccelController(game, Players.Count, player);
                controller.LoadContent(game.Content);
                Children.Add(controller);

                if (Players.Count == 0)
                {
                    var camController = new PlayfieldCamController(game, player.PlayerNum);
                    controller.LoadContent(game.Content);
                    Children.Add(camController);
                }

                Players.Add(player);
            }
        }
    }
}