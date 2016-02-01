using System;
using System.Numerics;

namespace SimdSpike {
    public static class IntSimdProcessor {
        public static void NaiveMinMax(int[] input, out int minimum, out int maximum) {
            int min = int.MaxValue, max = int.MinValue;
            foreach (var value in input) {
                min = Math.Min(min, value);
                max = Math.Max(max, value);
            }
            minimum = min;
            maximum = max;
        }

        public static void HWAcceleratedMinMax(int[] input, out int min, out int max) {
            var simdLength = Vector<int>.Count;
            var vmin = new Vector<int>(int.MaxValue);
            var vmax = new Vector<int>(int.MinValue);
            var i = 0;
            var lastSafeVectorIndex = input.Length - simdLength;
            for (i = 0; i < lastSafeVectorIndex; i += simdLength) {
                var va = new Vector<int>(input, i);
                vmin = Vector.Min(va, vmin);
                vmax = Vector.Max(va, vmax);
            }
            min = int.MaxValue;
            max = int.MinValue;
            for (var j = 0; j < simdLength; ++j) {
                min = Math.Min(min, vmin[j]);
                max = Math.Max(max, vmax[j]);
            }
            for (; i < input.Length; ++i) {
                min = Math.Min(min, input[i]);
                max = Math.Max(max, input[i]);
            }
        }
    }
}