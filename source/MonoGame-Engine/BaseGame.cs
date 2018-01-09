using Microsoft.Xna.Framework;
using MonoGame_Engine.Engine.Input;
using XnaInput = Microsoft.Xna.Framework.Input;

namespace MonoGame_Engine.Engine
{
    public class BaseGame : Game
    {
        protected readonly GraphicsDeviceManager graphics;

        public readonly Screen Screen;
        public readonly Scenes Scenes;
        public readonly Fonts Fonts;
        public readonly Inputs Inputs;

        public BaseGame()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);

            Screen = new Screen(graphics);
            Scenes = new Scenes(this);
            Fonts = new Fonts(this);
            Inputs = new Inputs();
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
                if (Inputs.Player(i).WasPressed(Buttons.ToggleFullscreen))
                {
                    Screen.ToggleFullscreen();
                    break;
                }
            }

            return Inputs.NumPlayers;
        }

        protected override void Update(GameTime gameTime)
        {
            if (Scenes.Current != null)
                Scenes.Current.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Screen.PreDraw();

            if (Scenes.Current != null)
                Scenes.Current.Draw(gameTime);

            base.Draw(gameTime);

            Screen.PostDraw();
        }

    }
}
