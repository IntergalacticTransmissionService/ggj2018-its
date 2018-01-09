using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Engine.Engine;
using MonoGame_Engine.Engine.Input;

namespace MonoGame_Shared
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MonoGame : BaseGame
    {
        public MonoGame() : base()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            for (int i = 0; i < 4; ++i)
                Inputs[i].Provider = new XBoxController(i);
            for (int i = 4; i < 5; ++i)
                Inputs[i].Provider = new KeyboardController(i);


            MainScene scene = new MainScene(this);
            Scenes.Add("Main", scene);

            Scenes.Show("Main");

#if !DEBUG
            Screen.ToggleFullscreen();
#endif
        }
    }
}
