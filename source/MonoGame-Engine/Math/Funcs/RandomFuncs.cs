using System;

namespace MonoGame_Engine.Math.Funcs
{
    public static class RandomFuncs
    {
        private static Random rng = new Random();

        public static float FromRange(float min, float max)
        {
            return (float)(min + rng.NextDouble() * (max - min));
        }
    }
}
