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
                result[i] = (ushort)(lhs[i] + rhs[i]);
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
                result[i] = (ushort)(lhs[i] + rhs[i]);
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

    }
}