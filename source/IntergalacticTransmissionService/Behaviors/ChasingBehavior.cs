using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace IntergalacticTransmissionService.Behaviors
{
    class ChasingBehavior : Behavior
    {
        public GameObject Target;
        public MainScene Scene { get; }
        public float ChaseDist { get; }
        public float ForgetDist { get; }
        public float Spd { get; }

        public ChasingBehavior(MainScene scene, float chaseDist, float forgetDist, float spd)
        {
            this.Scene = scene;
            this.ChaseDist = chaseDist;
            this.ForgetDist = forgetDist;
            this.Spd = spd;
        }


        public override void Update(GameObject owner, GameTime gameTime)
        {
            if (Target == null)
            {
                // search nearest Player or track parcel
                Player nearest = null;
                float nearestDist = float.MaxValue;
                foreach (var player in Scene.Players)
                {
                    var dist = Vector2.Distance(player.Phy.Pos, owner.Phy.Pos);
                    if (dist < nearestDist && dist < ChaseDist && player.IsAlive)
                    {
                        nearestDist = dist;
                        nearest = player;
                    }
                }
                Target = nearest;

                if (Target == null)
                {
                    var dist = Vector2.Distance(Scene.Parcel.Phy.Pos, owner.Phy.Pos);
                    if (dist > ForgetDist)
                        Target = Scene.Parcel;
                }
            } else
            {
                var dist = Vector2.Distance(Target.Phy.Pos, owner.Phy.Pos);
                if (Target is Player)
                {
                    if (dist > ForgetDist || !((Target as Player)?.IsAlive ?? false))
                        Target = null;
                } else
                {
                    if (dist < ChaseDist)
                        Target = null;
                }
            }

            if (Target != null && owner != null)
            {
                var spd = Target is Player ? Spd : Spd * 0.3f;

                owner.Phy.Dmp = 1f;
                owner.Phy.Spd = Vector2.Normalize(Target.Phy.Pos - owner.Phy.Pos) * spd;
            } else
            {
                owner.Phy.Dmp = 0.99f;
            }
        }

        public override void Reset()
        {
            this.Target = null;
        }
    }
}
