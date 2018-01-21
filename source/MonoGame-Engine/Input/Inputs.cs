using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoGame_Engine.Input
{
    public class Inputs
    {
        private readonly List<InputState> states;
        private readonly Dictionary<int, int> playerMap;

        public int NumPlayers { get { return playerMap.Count; } }
        public int NumPlayersAvailable {  get { return states.Count; } }

        public Inputs()
        {
            states = new List<InputState>();
            playerMap = new Dictionary<int, int>();
        }

        public void Add(InputProvider provider)
        {
            var state = new InputState();
            state.Provider = provider;
            states.Add(state);
        }

        public InputState Player(int playerNo)
        {
            if (playerMap.ContainsKey(playerNo))
                return states[playerMap[playerNo]];

            return null;
        }

        public void ReAssignToPlayers()
        {
            playerMap.Clear();
        }

        public bool AssignToPlayer(int playerNo)
        {
            for(int i=0; i<states.Count; ++i)
            {
                var inputState = states[i];
                if (!playerMap.ContainsValue(i) && inputState.AnyButtonDown)
                {
                    playerMap[playerNo] = i;
                    Console.WriteLine(string.Format("Assigned Controller {0} to Player {1}", i, playerNo));
                    return true;
                }
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            foreach(var input in states)
                input.Update(gameTime);
        }
    }
}
