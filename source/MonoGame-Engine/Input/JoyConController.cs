using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using JoyCon;
using Microsoft.Xna.Framework;

namespace MonoGame_Engine.Input
{
    public class JoyConController : InputProvider
    {
        private static JoyconManager manager = new JoyconManager();
        private readonly int index;

        public JoyConController(int index)
        {
            this.index = index;
        }

        public override bool Get(Buttons btn)
        {
            var joycon = JoyCon;
            if (joycon != null)
            {
                switch (btn)
                {
                    case Buttons.A: return JoyCon.GetButtonDown(joycon.isLeft ? Joycon.Button.DPAD_LEFT : Joycon.Button.DPAD_RIGHT);
                    case Buttons.B: return JoyCon.GetButtonDown(joycon.isLeft ? Joycon.Button.DPAD_DOWN : Joycon.Button.DPAD_UP);
                    case Buttons.X: return JoyCon.GetButtonDown(joycon.isLeft ? Joycon.Button.DPAD_UP : Joycon.Button.DPAD_DOWN);
                    case Buttons.Y: return JoyCon.GetButtonDown(joycon.isLeft ? Joycon.Button.DPAD_RIGHT : Joycon.Button.DPAD_LEFT);

                    case Buttons.R: return JoyCon.GetButtonDown(Joycon.Button.SR);
                    case Buttons.L: return JoyCon.GetButtonDown(Joycon.Button.SL);

                    // TODO    
                    //case Buttons.LeftStick: return st.Buttons.LeftStick == XnaInput.ButtonState.Pressed;
                    //case Buttons.RightStick: return st.Buttons.RightStick == XnaInput.ButtonState.Pressed;

                    //case Buttons.DPad_Left: return st.DPad.Left == XnaInput.ButtonState.Pressed;
                    //case Buttons.DPad_Right: return st.DPad.Right == XnaInput.ButtonState.Pressed;
                    //case Buttons.DPad_Up: return st.DPad.Up == XnaInput.ButtonState.Pressed;
                    //case Buttons.DPad_Down: return st.DPad.Down == XnaInput.ButtonState.Pressed;

                    //case Buttons.Select: return st.Buttons.Back == XnaInput.ButtonState.Pressed;
                    //case Buttons.Start: return st.Buttons.Start == XnaInput.ButtonState.Pressed;
                }
            }
            return false;
        }

        public override float Get(Sliders sldr)
        {
            var joycon = JoyCon;
            if (joycon != null)
            {
                var stick = joycon.GetStick();
                switch (sldr)
                {
                    case Sliders.LeftStickX: return joycon.isLeft ? -stick[1] : stick[1];
                    case Sliders.LeftStickY: return joycon.isLeft ? -stick[0] : stick[0];
                        //case Sliders.RightStickX: return st.ThumbSticks.Right.X;
                        //case Sliders.RightStickY: return -st.ThumbSticks.Right.Y;
                        //case Sliders.LeftTrigger: return st.Triggers.Left;
                        //case Sliders.RightTrigger: return st.Triggers.Right;
                }
            }
            return 0.0f;
        }

        public override void Rumble(float low, float high, int ms)
        {
            var joycon = JoyCon;
            if (joycon != null)
            {
                joycon.SetRumble(low, high, 0.5f, ms);
            }
        }

        private Joycon JoyCon { get { return manager.JoyCons.Count >= index + 1 ? manager.JoyCons[index] : null; } }

        public override void Update(GameTime gameTime)
        {
            if (index == 0)
            {
                manager.Update();
                manager.RefreshJoyConList();
            }
        }
    }
}
