using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SimdSpike {
    public class UShortSimdProcessor {
        public static void NaiveSumUnchecked(ushort[] lhs, ushort[] rhs, ushort[] result) {
            var length = lhs.Length;
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

        public static void HwAcceleratedSumUnchecked(ushort[] lhs, ushort[] rhs, ushort[] result) {
            var simdLength = Vector<ushort>.Count;
            var i = 0;
            for (i = 0; i < lhs.Length - simdLength; i += simdLength) {
                var va = new Vector<ushort>(lhs, i);
                var vb = new Vector<ushort>(rhs, i);
                (va + vb).CopyTo(result, i);
            }
            for (; i < lhs.Length; ++i) {
                result[i] = (ushort)(lhs[i] + rhs[i]);
            }
        }

        internal static void NaiveMinMax(ushort[] input, out ushort minimum, out ushort maximum) {
            var min = ushort.MaxValue;
            var max = ushort.MinValue;
            foreach (var value in input) {
                min = Math.Min(min, value);
                max = Math.Max(max, value);
            }
            minimum = min;
            maximum = max;
        }

        internal static void HWAcceleratedMinMax(ushort[] input, out ushort min, out ushort max) {
            var simdLength = Vector<ushort>.Count;
            var vmin = new Vector<ushort>(ushort.MaxValue);
            var vmax = new Vector<ushort>(ushort.MinValue);
            var i = 0;
            var lastSafeVectorIndex = input.Length - simdLength;
            for (i = 0; i < lastSafeVectorIndex; i += simdLength) {
                var va = new Vector<ushort>(input, i);
                vmin = Vector.Min(va, vmin);
                vmax = Vector.Max(va, vmax);
            }
            min = ushort.MaxValue;
            max = ushort.MinValue;
            for (var j = 0; j < simdLength; ++j) {
                min = Math.Min(min, vmin[j]);
                max = Math.Max(max, vmax[j]);
            }
            for (; i < input.Length; ++i) {
                min = Math.Min(min, input[i]);
                max = Math.Max(max, input[i]);
            }
        }

        internal static ulong NaiveTotalOfArray(ushort[] input) {
            ulong total = 0;
            foreach (var value in input) {
                total += value;
            }

            return total;
        }

        internal static ushort NaiveUncheckedTotalOfArray(ushort[] input) {
            var result = 0;
            unchecked {
                foreach (var value in input)
                    result = result + value;
            }
            return (ushort)result;
        }

        internal static ulong HWAcceleratedTotalOfArray(ushort[] input) {
            var uintArray = Array.ConvertAll(input, x => (ulong) x);
            var simdLength = Vector<ulong>.Count;
            var vTotal = new Vector<ulong>(0);
            var i = 0;
            var lastSafeVectorIndex = uintArray.Length - simdLength;
            for (i = 0; i < lastSafeVectorIndex; i += simdLength) {
                var vector = new Vector<ulong>(uintArray, i);
                vTotal = Vector.Add(vTotal, vector);
            }
            ulong total = 0;
            for (var j = 0; j < simdLength; ++j) {
                total += vTotal[j];
            }
            for (; i < input.Length; ++i) {
                total += input[i];
            }
            return total;
        }

        internal static ushort HWAcceleratedUncheckedTotalOfArray(ushort[] input) {
            var simdLength = Vector<ushort>.Count;
            var vTotal = new Vector<ushort>(0);
            var i = 0;
            var lastSafeVectorIndex = input.Length - simdLength;
            for (i = 0; i < lastSafeVectorIndex; i += simdLength) {
                var vector = new Vector<ushort>(input, i);
                vTotal = Vector.Add(vTotal, vector);
            }
            ushort total = 0;
            for (var j = 0; j < simdLength; ++j) {
                total += vTotal[j];
            }
            for (; i < input.Length; ++i) {
                total += input[i];
            }
            return total;
        }

        internal static void NaiveGetStats(ushort[] input, out ushort min, out ushort max, out double average) {
            min = ushort.MaxValue;
            max = ushort.MinValue;
            ulong total = 0;
            foreach (var value in input) {
                if (value < min) min = value;
                if (value > max) max = value;
                total += value;
            }
            average = total / (double)input.Length;
        }

        internal static void HWAcceleratedGetStats(ushort[] input, out ushort min, out ushort max, out double average) {
            var simdLength = Vector<ushort>.Count;
            var vmin = new Vector<ushort>(ushort.MaxValue);
            var vmax = new Vector<ushort>(ushort.MinValue);
            var i = 0;
            ulong total = 0;
            var lastSafeVectorIndex = input.Length - simdLength;
            for (i = 0; i < lastSafeVectorIndex; i += simdLength) {
                var va = new Vector<ushort>(input, i);
                vmin = Vector.Min(va, vmin);
                vmax = Vector.Max(va, vmax);
                for (var j = i; j < i + simdLength; j++) {
                    total += input[j];
                }
            }
            min = ushort.MaxValue;
            max = ushort.MinValue;
            for (var j = 0; j < simdLength; ++j) {
                min = Math.Min(min, vmin[j]);
                max = Math.Max(max, vmax[j]);
            }
            for (; i < input.Length; ++i) {
                min = Math.Min(min, input[i]);
                max = Math.Max(max, input[i]);
                total += input[i];
            }
            average = total/(double) input.Length;
        }
    }
}