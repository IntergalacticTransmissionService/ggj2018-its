using Microsoft.Xna.Framework;
using MonoGame_Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntergalacticTransmissionService
{
    public class CollisionHandler
    {
        private readonly List<Player> players;

        public CollisionHandler(List<Player> players)
        {
            this.players = players;
        }

        public void Update(GameTime gameTime)
        {
            for(int i=0; i<players.Count; ++i)
            {
                var left = players[i];
                for (int j=i+1; j<players.Count; ++j)
                {
                    var right = players[j];

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
            }
        }
    }
}
