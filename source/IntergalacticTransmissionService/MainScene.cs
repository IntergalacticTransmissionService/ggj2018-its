using IntergalacticTransmissionService.Input;
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

        internal readonly Parcel Parcel;
        internal readonly List<Player> Players;
        internal readonly CollisionHandler CollisionHandler;

        internal new ITSGame game {  get { return base.game as ITSGame; } }

        public MainScene(ITSGame game) : base(game)
        {
            BackgroundImg = new TilingImage("Images/starfield.png", game);
            Background = new Sprite(BackgroundImg);
            Parcel = new Parcel(game, Color.LightPink, 32f);
            Players = new List<Player>();
            CollisionHandler = new CollisionHandler(this);
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

            Parcel.LoadContent(game.Content);
            Parcel.Phy.Pos.X = 500;
            Children.Add(Parcel);
        }

        internal override void Update(GameTime gameTime)
        {
            CollisionHandler.Update(gameTime);
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
                var rnd = new Random();
                var player = new Player(game, Players.Count, 32f);
                player.LoadContent(game.Content);
                player.Phy.Pos.X = (float)(rnd.NextDouble() - 0.5) * 100;
                player.Phy.Pos.Y = (float)(rnd.NextDouble() - 0.5) * 100;
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
