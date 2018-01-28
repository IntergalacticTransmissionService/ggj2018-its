using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Input;
using System.IO;
using XnaInput = Microsoft.Xna.Framework.Input;

namespace MonoGame_Engine
{
    public class BaseGame : Game
    {
#if DEBUG
        public static Texture2D debug;
#endif

        protected readonly GraphicsDeviceManager graphics;

        public readonly Screen Screen;
        public readonly Camera Camera;
        public readonly Scenes Scenes;
        public readonly Fonts Fonts;
        public readonly Inputs Inputs;
        public readonly DebugOverlay DebugOverlay;

        public new LiveContentManager Content { get { return base.Content as LiveContentManager; } }

        public BaseGame()
        {
            base.Content = new LiveContentManager(this);

            graphics = new GraphicsDeviceManager(this);

            Screen = new Screen(graphics, 1920, 1080);
            Camera = new Camera(this);
            Scenes = new Scenes(this);
            Fonts = new Fonts(this);
            Inputs = new Inputs();
            DebugOverlay = new DebugOverlay(this);
        }

        protected override void Initialize()
        {
            base.Initialize();
            Screen.Initialize();
        }

        protected override void LoadContent()
        {
#if DEBUG
            debug = this.Content.Load<Texture2D>("Images/particle.png"); ;
#endif
            Fonts.LoadFonts();
            DebugOverlay.LoadContent();
        }

        protected override void UnloadContent()
        {
            Scenes.Clear();
            Fonts.Clear();
        }

        bool wasF3pressed = false;
        bool isF3pressed = false;
        internal virtual int HandleInput(GameTime gameTime)
        {
            if (XnaInput.Keyboard.GetState().IsKeyDown(XnaInput.Keys.Escape))
                Exit();

            wasF3pressed = isF3pressed;
            isF3pressed = XnaInput.Keyboard.GetState().IsKeyDown(XnaInput.Keys.F3);
            if (wasF3pressed && !isF3pressed)
                Screen.ScreenShot();

            if (XnaInput.Keyboard.GetState().IsKeyDown(XnaInput.Keys.F5))
                Content.ReloadAll();

            Inputs.Update(gameTime);


            for (int i = 0; i < Inputs.NumPlayers; i++)
            {
                if (Inputs.Player(i).WasPressed(Buttons.Select))
                {
                    Screen.ToggleFullscreen();
                    break;
                }
            }

            return Inputs.NumPlayers;
        }

        protected override void Update(GameTime gameTime)
        {
            DebugOverlay.Text = "";
            Content.Update(gameTime);
            Camera.Update(gameTime);
            Scenes.Current?.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Screen.PreDraw();
            Scenes.Current?.Draw(gameTime, Camera);
            base.Draw(gameTime);
            Screen.PostDraw();
            DebugOverlay.Draw(gameTime);
        }
    }
}
