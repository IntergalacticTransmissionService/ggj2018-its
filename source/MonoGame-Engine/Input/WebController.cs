using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using IntergalacticTransmissionService.Net;
using Microsoft.Xna.Framework;

namespace MonoGame_Engine.Input
{
    public class WebController : InputProvider
    {
        private static WebControllerManager manager;
        private static HashSet<int> connected;
        private readonly int index;

        public WebController(int index)
        {
            if (manager == null)
            {
                manager = new WebControllerManager();
                connected = new HashSet<int>();
            }

            this.index = index;
            connected.Add(this.index);
        }

        public override bool Get(Buttons btn)
        {
            switch (btn)
            {
                case Buttons.A: return manager.getButton(index, 0);
                case Buttons.B: return manager.getButton(index, 1);
                case Buttons.X: return manager.getButton(index, 2);
                case Buttons.Y: return manager.getButton(index, 3);

                // TODO    
                // case Buttons.R: return false;
                // case Buttons.L: return false;

                //case Buttons.LeftStick: return st.Buttons.LeftStick == XnaInput.ButtonState.Pressed;
                //case Buttons.RightStick: return st.Buttons.RightStick == XnaInput.ButtonState.Pressed;

                //case Buttons.DPad_Left: return st.DPad.Left == XnaInput.ButtonState.Pressed;
                //case Buttons.DPad_Right: return st.DPad.Right == XnaInput.ButtonState.Pressed;
                //case Buttons.DPad_Up: return st.DPad.Up == XnaInput.ButtonState.Pressed;
                //case Buttons.DPad_Down: return st.DPad.Down == XnaInput.ButtonState.Pressed;

                //case Buttons.Select: return st.Buttons.Back == XnaInput.ButtonState.Pressed;
                //case Buttons.Start: return st.Buttons.Start == XnaInput.ButtonState.Pressed;
            }
            return false;
        }

        public override float Get(Sliders sldr)
        {
            switch (sldr)
            {
                case Sliders.LeftStickX: return manager.getAxis(index, 0);
                case Sliders.LeftStickY: return manager.getAxis(index, 1);
                    //case Sliders.RightStickX: return st.ThumbSticks.Right.X;
                    //case Sliders.RightStickY: return -st.ThumbSticks.Right.Y;
                    //case Sliders.LeftTrigger: return st.Triggers.Left;
                    //case Sliders.RightTrigger: return st.Triggers.Right;
            }
            return 0.0f;
        }

        public override void Rumble(float low, float high, int ms)
        {
            manager.SetRumble(index, ms);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Dispose()
        {
            connected.Remove(this.index);
            
            if (connected.Count() == 0)
            {
                manager.Dispose();
                manager = null;
            }
        }
    }
}
