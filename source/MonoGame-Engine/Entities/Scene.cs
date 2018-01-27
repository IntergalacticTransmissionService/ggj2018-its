using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Engine.Entities
{
    public class Scene : Group
    {
        protected readonly BaseGame game;

        protected Color BgColor { get; set; }
        protected SpriteBatch spriteBatch;

        public Scene(BaseGame game)
        {
            this.game = game;
        }

        internal virtual void Initialize()
        {
            BgColor = Color.Black;
        }

        internal virtual void LoadContent()
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        internal virtual void UnloadContent()
        {

        }

        internal virtual int HandleInput(GameTime gameTime)
        {
            return game.HandleInput(gameTime);
        }

        internal override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);

            base.Update(gameTime);
        }

        internal override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            base.Draw(batch, gameTime);
        }

        internal virtual void Draw(GameTime gameTime, Camera cam)
        {
            game.GraphicsDevice.Clear(BgColor);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, cam.Matrix);
            Draw(spriteBatch, gameTime);
            spriteBatch.End();
        }
    }
}
