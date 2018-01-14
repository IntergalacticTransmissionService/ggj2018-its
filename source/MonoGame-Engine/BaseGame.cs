using Microsoft.Xna.Framework;
using MonoGame_Engine.Engine.Input;
using XnaInput = Microsoft.Xna.Framework.Input;

namespace MonoGame_Engine.Engine
{
    public class BaseGame : Game
    {
        protected readonly GraphicsDeviceManager graphics;

        public readonly Screen Screen;
        public readonly Camera Camera;
        public readonly Scenes Scenes;
        public readonly Fonts Fonts;
        public readonly Inputs Inputs;
        public readonly DebugOverlay DebugOverlay;

        public BaseGame()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);

            Screen = new Screen(graphics);
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

            for (int i = 0; i < 5; ++i)
                Inputs.Add(new InputState());
        }

        protected override void LoadContent()
        {
            Fonts.LoadFonts();
            DebugOverlay.LoadContent();
        }

        protected override void UnloadContent()
        {
            Scenes.Clear();
            Fonts.Clear();
        }

        internal virtual int HandleInput(GameTime gameTime)
        {
            if (XnaInput.Keyboard.GetState().IsKeyDown(XnaInput.Keys.Escape))
                Exit();

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
