using System;

namespace MonoGame_Engine.Math
{
    public static class RandomFuncs
    {
        private static Random rng = new Random();

        public static float FromRange(float min, float max)
        {
            return (float)(min + rng.NextDouble() * (max - min));
        }

        internal static int FromRangeInt(int min, int max) => min + rng.Next(max - min);
    }
}
