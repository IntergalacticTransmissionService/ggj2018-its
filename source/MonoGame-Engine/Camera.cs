using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Phy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Engine
{
    public class Camera : IHasPhysics
    {
        private BaseGame game;
        private Matrix matrix;

        public Physics Phy { get; private set; }
        public float Zoom { get; set; }

        public Camera(BaseGame game) : base()
        {
            this.game = game;
            this.Phy = new Physics(0);
            this.Zoom = 1.0f;
        }

        internal virtual void Update(GameTime gameTime)
        {
            this.Phy?.Update(gameTime);

            matrix = Matrix.CreateTranslation(new Vector3(-Phy.Pos.X, -Phy.Pos.Y, 0)) *
                                                     Matrix.CreateRotationZ(-Phy.Rot) *
                                                     Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                                     Matrix.CreateTranslation(new Vector3(game.Screen.CanvasWidth * 0.5f, game.Screen.CanvasHeight * 0.5f, 0));
        }

        public Matrix Matrix { get { return matrix; } set { matrix = value; } }
        public Vector2 TopLeft { get { return Vector2.Transform(Vector2.Zero, Matrix.Invert(Matrix)); } }
        public Vector2 TopRight { get { return Vector2.Transform(new Vector2(game.Screen.CanvasWidth, 0), Matrix.Invert(Matrix)); } }
        public Vector2 BottomLeft { get { return Vector2.Transform(new Vector2(0, game.Screen.CanvasHeight), Matrix.Invert(Matrix)); } }
        public Vector2 BottomRight { get { return Vector2.Transform(new Vector2(game.Screen.CanvasWidth, game.Screen.CanvasHeight), Matrix.Invert(Matrix)); } }
    }
}
