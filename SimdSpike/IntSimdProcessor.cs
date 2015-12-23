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

        public static void HWAcceleratedMinMax(int[] input, out int minimum, out int maximum) {
            var simdLength = Vector<int>.Count;
            var vmin = new Vector<int>(int.MaxValue);
            var vmax = new Vector<int>(int.MinValue);
            for (var i = 0; i < input.Length; i += simdLength) {
                var va = new Vector<int>(input, i);
                var vLessThan = Vector.LessThan(va, vmin);
                vmin = Vector.ConditionalSelect(vLessThan, va, vmin);
                var vGreaterThan = Vector.GreaterThan(va, vmax);
                vmax = Vector.ConditionalSelect(vGreaterThan, va, vmax);
            }
            int min = int.MaxValue, max = int.MinValue;
            for (int i = 0; i < simdLength; ++i) {
                min = Math.Min(min, vmin[i]);
                max = Math.Max(max, vmax[i]);
            }
            minimum = min;
            maximum = max;
        }
    }
}