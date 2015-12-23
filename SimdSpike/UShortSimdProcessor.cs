using System;
using System.Numerics;

namespace SimdSpike {
    public class UShortSimdProcessor {
        public static void NaiveSumUnchecked(ushort[] lhs, ushort[] rhs, ushort[] result) {
            var length = lhs.Length;
            ulong a = 4;
            ulong b = 6;
            ulong c = a + b;
            for (var i = 0; i < length; ++i) {
                result[i] = (ushort) (lhs[i] + rhs[i]);
            }
        }

        public static void NaiveSumInPlaceUnchecked(ushort[] lhs, ushort[] rhs) {
            var length = lhs.Length;
            for (var i = 0; i < length; ++i) {
                lhs[i] += rhs[i];
            }
        }

        public static ushort[] NaiveSumFuncUnchecked(ushort[] lhs, ushort[] rhs) {
            var length = lhs.Length;
            var result = new ushort[length];
            for (var i = 0; i < length; ++i) {
                result[i] = (ushort) (lhs[i] + rhs[i]);
            }
            return result;
        }

        public static void HwAcceleratedSumInPlaceUnchecked(ushort[] lhs, ushort[] rhs) {
            int simdLength = Vector<ushort>.Count;
            int i = 0;
            for (i = 0; i < lhs.Length - simdLength; i += simdLength) {
                Vector<ushort> va = new Vector<ushort>(lhs, i);
                Vector<ushort> vb = new Vector<ushort>(rhs, i);
                va += vb;
                va.CopyTo(lhs, i);
            }
            for (; i < lhs.Length; ++i) {
                lhs[i] += rhs[i];
            }
        }

        internal static void NaiveMinMax(ushort[] input, out ushort minimum, out ushort maximum) {
            ushort min = ushort.MaxValue, max = ushort.MinValue;
            foreach (var value in input) {
                min = Math.Min(min, value);
                max = Math.Max(max, value);
            }
            minimum = min;
            maximum = max;
        }

        internal static void HWAcceleratedMinMax(ushort[] input, out ushort minimum, out ushort maximum) {
            var simdLength = Vector<ushort>.Count;
            var vmin = new Vector<ushort>(ushort.MaxValue);
            var vmax = new Vector<ushort>(ushort.MinValue);
            for (var i = 0; i < input.Length; i += simdLength) {
                var va = new Vector<ushort>(input, i);
                var vLessThan = Vector.LessThan(va, vmin);
                vmin = Vector.ConditionalSelect(vLessThan, va, vmin);
                var vGreaterThan = Vector.GreaterThan(va, vmax);
                vmax = Vector.ConditionalSelect(vGreaterThan, va, vmax);
            }
            ushort min = ushort.MaxValue, max = ushort.MinValue;
            for (var i = 0; i < simdLength; ++i) {
                min = Math.Min(min, vmin[i]);
                max = Math.Max(max, vmax[i]);
            }
            minimum = min;
            maximum = max;
        }
    }
}