using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static System.Console;
using static SimdSpike.MethodValidation;
using static SimdSpike.Utilities;
using Stopwatch = NUnit.Framework.Compatibility.Stopwatch;

namespace SimdSpike {
    internal class Program {
        private const int TestSetSize = 20000000;

        static void Main() {
            if (!Vector.IsHardwareAccelerated) {
                WriteLine("Hardware acceleration not supported.");
                return;
            }

            Write("Validating addition methods...");
            ValidateAdditionMethods(NaiveSum, HwAcceleratedSum);
            ValidateAdditionInPlaceMethods(NaiveSumInPlace, HwAcceleratedSumInPlace);
            ValidateAdditionFuncs(NaiveSumFunc);
            WriteLine(" methods correct.");

            Write("Hardware acceleration is supported, generating test data...");
            var one = Enumerable.Range(0, TestSetSize).Select(x => RandomFloat()).ToArray();
            var two = Enumerable.Range(0, TestSetSize).Select(x => RandomFloat()).ToArray();
            WriteLine(" done.");

            WriteLine("Performance testing...");
            var sw = new Stopwatch();
            var naiveTimesMs = new List<long>();
            var hwTimesMs = new List<long>();
            for (var i = 0; i < 5; i++) {
                var naiveSum = new float[one.Length];
                sw.Restart();
                NaiveSum(one, two, naiveSum);
                var naiveTimeMs = sw.ElapsedMilliseconds;
                naiveTimesMs.Add(naiveTimeMs);
                WriteLine($"Naive addition took:          {naiveTimeMs}ms (last value = {naiveSum[naiveSum.Length-1]}).");

                var hwSum = new float[one.Length];
                sw.Restart();
                HwAcceleratedSum(one, two, hwSum);
                var hwTimeMs = sw.ElapsedMilliseconds;
                hwTimesMs.Add(hwTimeMs);
                WriteLine($"HW accelerated addition took: {hwTimeMs}ms (last value = {hwSum[hwSum.Length - 1]}).");
            }

            WriteLine();
            WriteLine($"Naive method average time:          {naiveTimesMs.Average()}");
            WriteLine($"HW accelerated method average time: {hwTimesMs.Average()}");
        }

        private static void NaiveSum(float[] lhs, float[] hs, float[] result) {
            var length = lhs.Length;
            for (var i = 0; i < length; ++i) {
                result[i] = lhs[i] + hs[i];
            }
        }

        private static void NaiveSumInPlace(float[] lhs, float[] rhs) {
            var length = lhs.Length;
            for (var i = 0; i < length; ++i) {
                lhs[i] += rhs[i];
            }
        }

        private static float[] NaiveSumFunc(float[] lhs, float[] hs) {
            var length = lhs.Length;
            var result = new float[length];
            for (var i = 0; i < length; ++i) {
                result[i] = lhs[i] + hs[i];
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
