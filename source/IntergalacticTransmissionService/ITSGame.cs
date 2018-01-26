using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using MonoGame_Engine;
using MonoGame_Engine.Input;

namespace IntergalacticTransmissionService
{
    public class ITSGame : BaseGame
    {
        public ITSGame() : base()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            for (int i = 0; i < 4; ++i)
                Inputs.Add(new XBoxController(i));

            for (int i = 0; i < 8; ++i)
                Inputs.Add(new JoyConController(i));

            Inputs.Add(new KeyboardController());

            MainScene scene = new MainScene(this);
            Scenes.Add("Main", scene);
            Scenes.Show("Main");

#if !DEBUG
            Screen.ToggleFullscreen();
#endif
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Inputs.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
