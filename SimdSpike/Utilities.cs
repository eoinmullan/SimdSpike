using System;

namespace SimdSpike {
    public static class Utilities {
        private static readonly Random random = new Random();

        public static float RandomFloat() {
            var mantissa = (random.NextDouble() * 2.0) - 1.0;
            var exponent = Math.Pow(2.0, random.Next(-126, 128));
            return (float)(mantissa * exponent);
        }
    }
}