using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NUnit.Framework.Compatibility;
using static System.Console;
using static SimdSpike.MethodValidation;
using static SimdSpike.Utilities;

namespace SimdSpike {
    public class FloatSimdProcessor {
        public static void TestInPlaceAddition(int testSetSize) {
            WriteLine();
            Write("Testing floats, generating test data...");
            var floatsOne = Enumerable.Range(0, testSetSize).Select(x => RandomFloat()).ToArray();
            var floatsTwo = Enumerable.Range(0, testSetSize).Select(x => RandomFloat()).ToArray();
            WriteLine(" done, testing...");

            var sw = new Stopwatch();
            var naiveTimesMs = new List<long>();
            var hwTimesMs = new List<long>();
            for (var i = 0; i < 3; i++) {
                var floatsOneCopy = new float[floatsOne.Length];

                floatsOne.CopyTo(floatsOneCopy, 0);
                sw.Restart();
                HwAcceleratedSumInPlace(floatsOneCopy, floatsTwo);
                var hwTimeMs = sw.ElapsedMilliseconds;
                hwTimesMs.Add(hwTimeMs);
                WriteLine($"HW accelerated addition took: {hwTimeMs}ms (last value = {floatsOneCopy[floatsOneCopy.Length - 1]}).");

                floatsOne.CopyTo(floatsOneCopy, 0);
                sw.Restart();
                NaiveSumInPlace(floatsOneCopy, floatsTwo);
                var naiveTimeMs = sw.ElapsedMilliseconds;
                naiveTimesMs.Add(naiveTimeMs);
                WriteLine($"Naive addition took:          {naiveTimeMs}ms (last value = {floatsOneCopy[floatsOneCopy.Length - 1]}).");
            }

            WriteLine("Testing floats");
            WriteLine($"Naive method average time:          {naiveTimesMs.Average()}");
            WriteLine($"HW accelerated method average time: {hwTimesMs.Average()}");
            WriteLine($"Hardware speedup:                   {(naiveTimesMs.Average() / hwTimesMs.Average()) * 100:0.00}%");
        }

        public static void ValidateAdditionMethods() {
            ValidateFloatAdditionMethods(NaiveSum, HwAcceleratedSum);
            ValidateFloatAdditionInPlaceMethods(NaiveSumInPlace, HwAcceleratedSumInPlace);
            ValidateFloatAdditionFuncs(NaiveSumFunc);
        }

        private static void NaiveSum(float[] lhs, float[] rhs, float[] result) {
            var length = lhs.Length;
            for (var i = 0; i < length; ++i) {
                result[i] = lhs[i] + rhs[i];
            }
        }

        private static void NaiveSumInPlace(float[] lhs, float[] rhs) {
            var length = lhs.Length;
            for (var i = 0; i < length; ++i) {
                lhs[i] += rhs[i];
            }
        }

        private static float[] NaiveSumFunc(float[] lhs, float[] rhs) {
            var length = lhs.Length;
            var result = new float[length];
            for (var i = 0; i < length; ++i) {
                result[i] = lhs[i] + rhs[i];
            }
            return result;
        }

        private static void HwAcceleratedSum(float[] lhs, float[] rhs, float[] result) {
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

        private static void HwAcceleratedSumInPlace(float[] lhs, float[] rhs) {
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