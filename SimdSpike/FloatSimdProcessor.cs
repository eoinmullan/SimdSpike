using System.Numerics;

namespace SimdSpike {
    public class FloatSimdProcessor {
        public static void NaiveSum(float[] lhs, float[] rhs, float[] result) {
            var length = lhs.Length;
            for (var i = 0; i < length; ++i) {
                result[i] = lhs[i] + rhs[i];
            }
        }

        public static void NaiveSumInPlace(float[] lhs, float[] rhs) {
            var length = lhs.Length;
            for (var i = 0; i < length; ++i) {
                lhs[i] += rhs[i];
            }
        }

        public static float[] NaiveSumFunc(float[] lhs, float[] rhs) {
            var length = lhs.Length;
            var result = new float[length];
            for (var i = 0; i < length; ++i) {
                result[i] = lhs[i] + rhs[i];
            }
            return result;
        }

        public static void HwAcceleratedSum(float[] lhs, float[] rhs, float[] result) {
            var simdLength = Vector<float>.Count;
            int i;
            for (i = 0; i < lhs.Length - simdLength; i += simdLength) {
                var va = new Vector<float>(lhs, i);
                var vb = new Vector<float>(rhs, i);
                va += vb;
                va.CopyTo(result, i);
            }

            for (; i < lhs.Length; ++i) {
                result[i] = lhs[i] + rhs[i];
            }
        }

        public static void HwAcceleratedSumInPlace(float[] lhs, float[] rhs) {
            int simdLength = Vector<float>.Count;
            int i = 0;
            for (i = 0; i < lhs.Length - simdLength; i += simdLength) {
                Vector<float> va = new Vector<float>(lhs, i);
                Vector<float> vb = new Vector<float>(rhs, i);
                va += vb;
                va.CopyTo(lhs, i);
            }
            for (; i < lhs.Length; ++i) {
                lhs[i] += rhs[i];
            }
        }
    }
}