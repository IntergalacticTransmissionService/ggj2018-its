using Microsoft.Xna.Framework;
using MonoGame_Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntergalacticTransmissionService
{
    public class CollisionHandler
    {
        private readonly MainScene scene;

        public CollisionHandler(MainScene scene)
        {
            this.scene = scene;
        }

        public void Update(GameTime gameTime)
        {
            for(int i=0; i<scene.Players.Count; ++i)
            {
                var left = scene.Players[i];

                // check Parcel
                if (left.Phy.CollidesWith(scene.Parcel.Phy))
                {
                    if (scene.Parcel.HoldBy == null)
                        scene.Parcel.HoldBy = left;
                }

                // check other players
                for (int j=0; j< scene.Players.Count; ++j)
                {
                    var right = scene.Players[j];

                    // collisions with other players
                    if (j > i)
                    {
                        if (left.Phy.CollidesWith(right.Phy))
                        {
                            left.WasHit();
                            right.WasHit();

                            float dist = Vector2.Distance(left.Phy.Pos, right.Phy.Pos);
                            float correction = 0.5f * ((left.Radius + right.Radius) - dist);
                            Vector2 vec = Vector2.Normalize(Vector2.Subtract(right.Phy.Pos, left.Phy.Pos));
                            left.Phy.Pos -= 2 * correction * vec;
                            right.Phy.Pos += 2 * correction * vec;

                            left.Phy.Spd -= 100 * vec;
                            left.Phy.Accel -= 10000 * vec;

                            right.Phy.Spd += 100 * vec;
                            right.Phy.Accel += 10000 * vec;
                        }
                    }

                    // collision with other players bullets
                    if (i != j)
                    {
                        int k = 0;
                        foreach (var bullet in right.Bullets)
                        {
                            if (left.Phy.CollidesWith(bullet))
                            {
                                right.Bullets.Remove(k);
                                left.WasHit();
                            }
                            ++k;
                        }
                    }
                }
            }
        }
    }
}
