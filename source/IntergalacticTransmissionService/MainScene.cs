﻿using IntergalacticTransmissionService.Behaviors;
using IntergalacticTransmissionService.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine;
using MonoGame_Engine.Entities;
using MonoGame_Engine.Gfx;
using MonoGame_Engine.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntergalacticTransmissionService
{
    public class MainScene : Scene
    {
        internal readonly TilingImage BackgroundImg;
        internal Sprite Background;
        internal readonly Leviathan Leviathan;
        internal readonly Parcel Parcel;
        internal readonly List<Player> Players;
        internal readonly List<Enemy> Enemies;
        internal readonly CollisionHandler CollisionHandler;

        internal readonly Level Level;
        internal readonly HUD Hud;



        internal new ITSGame game { get { return base.game as ITSGame; } }

        public MainScene(ITSGame game) : base(game)
        {
            BackgroundImg = new TilingImage("Images/starfield.png", game);
            Background = new Sprite(BackgroundImg, -1);
            Parcel = new Parcel(game, Color.White, 32f);
            Players = new List<Player>();
            Enemies = new List<Enemy>();
            CollisionHandler = new CollisionHandler(this);
            Level = new Level(game, 300, 1000, 30, 40, TimeSpan.FromSeconds(2));

            Leviathan = new Leviathan(game, Color.White, 400, game.Camera.Phy.Pos, MathHelper.PiOver2, new LeviathanBehavior(this));

            Hud = new HUD(game);
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
            Level.LoadContent(game.Content);
            Children.Add(Level);

            Leviathan.LoadContent(game.Content);
            this.Children.Add(Leviathan);

            foreach (var e in Enemies) { e.LoadContent(game.Content); }
            Hud.LoadContent(game.Content);
        }

        internal override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            base.Draw(batch, gameTime);
            foreach (var e in Enemies) { e.Draw(batch, gameTime); }
            Hud.Draw(batch, gameTime);
        }

        internal override void Update(GameTime gameTime)
        {
            foreach (var e in Enemies) { e.Update(gameTime); }
            CollisionHandler.Update(gameTime);
            base.Update(gameTime);

            CheckForNewPlayers();
            Hud.Update(gameTime);

            Enemies.RemoveAll(e => !e.IsAlive);
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
                if (Players.Count == 0)
                    Leviathan.Start();

                var rnd = new Random();
                var player = new Player(game, Players.Count, 32f);
                player.LoadContent(game.Content);
                player.Spawn();
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
