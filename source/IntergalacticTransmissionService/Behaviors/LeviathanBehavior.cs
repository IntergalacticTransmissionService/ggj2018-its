using IntergalacticTransmissionService.Behaviors;
using Microsoft.Xna.Framework;
using System;

namespace IntergalacticTransmissionService
{
    internal class LeviathanBehavior : Behavior
    {
        public MainScene Scene { get; }

        private TimeSpan orientationTime;
        private int steering;

        public LeviathanBehavior(MainScene scene)
        {
            this.Scene = scene;
        }


        public override void Update(GameObject owner, GameTime gameTime)
        {
            var rotationMatrix = Matrix.CreateRotationZ(owner.Phy.Rot);
            owner.Phy.Spd = -Vector2.Transform(Vector2.UnitY, rotationMatrix) * (Player.DefaultMaxSpd * 0.7f);
            //owner.Phy.Spd = Vector2.UnitX * 30;
            //owner.Phy.Spd = -Vector2.UnitY * 30;
            const float maxTurnspeed = MathHelper.PiOver4 / 40;

            if (orientationTime.Ticks < 0)
            {
                orientationTime = TimeSpan.FromSeconds(15);
                steering = MonoGame_Engine.Math.RandomFuncs.FromRangeInt(-1, 1);
            }
            orientationTime -= gameTime.ElapsedGameTime;
            switch (steering)
            {
                case 1:
                    owner.Phy.RotSpd += MonoGame_Engine.Math.RandomFuncs.FromRange(0, maxTurnspeed / 10);
                    break;
                default:
                case 0:
                    owner.Phy.RotSpd = 0;
                    break;
                case -1:
                    owner.Phy.RotSpd += MonoGame_Engine.Math.RandomFuncs.FromRange(-maxTurnspeed / 10, 0);
                    break;
            }

            owner.Phy.RotSpd = MathHelper.Clamp(owner.Phy.RotSpd, -maxTurnspeed, maxTurnspeed);

            
        }

        public override void Reset()
        {
        }
    }
}
