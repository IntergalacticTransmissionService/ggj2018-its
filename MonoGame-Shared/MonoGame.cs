using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_Shared
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MonoGame : Game
    {
        PlatformDefs platformDefs;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D image;
        Rectangle screen;
        
        public MonoGame(PlatformDefs platformDefs)
        {
            this.platformDefs = platformDefs;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            GraphicsDevice.PresentationParameters.BackBufferWidth = platformDefs.Width;
            GraphicsDevice.PresentationParameters.BackBufferHeight = platformDefs.Height;
            GraphicsDevice.PresentationParameters.IsFullScreen = platformDefs.FullScreen;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            image = Content.Load<Texture2D>("Images/image1920x1080");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (screen == null)
                screen = new Rectangle(0, 0, 0, 0);

            screen.X = 0;
            screen.Y = 0;
            screen.Width = GraphicsDevice.PresentationParameters.BackBufferWidth;
            screen.Height = GraphicsDevice.PresentationParameters.BackBufferHeight;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            spriteBatch.Draw(image, screen, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
