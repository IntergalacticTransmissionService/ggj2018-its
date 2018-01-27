using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Engine.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntergalacticTransmissionService
{
    public class Parcel : Sprite
    {
        public Player HoldBy { get; set; }

        public Parcel() : base("Images/parcel.png")
        {
        }

        internal override void Update(GameTime gameTime)
        {
            if (HoldBy != null)
            {
                Phy.Pos = HoldBy.Phy.Pos;
            }
            base.Update(gameTime);
        }
    }
}
