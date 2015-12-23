using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NUnit.Framework.Compatibility;
using static System.Console;
using static SimdSpike.Utilities;

namespace SimdSpike {
    public class UShortSimdProcessor {
        public static void TestInPlaceAddition(int testSetSize) {
            WriteLine();
            Write("Testing floats, generating test data...");
            var ushortsOne = GetRandomTestData(testSetSize);
            var ushortsTwo = GetRandomTestData(testSetSize);
            WriteLine(" done, testing...");

            var sw = new Stopwatch();
            var naiveTimesMs = new List<long>();
            var hwTimesMs = new List<long>();
            for (var i = 0; i < 3; i++) {
                var ushortsOneCopy = new ushort[ushortsOne.Length];

                ushortsOne.CopyTo(ushortsOneCopy, 0);
                sw.Restart();
                HwAcceleratedSumInPlaceUnchecked(ushortsOneCopy, ushortsTwo);
                var hwTimeMs = sw.ElapsedMilliseconds;
                hwTimesMs.Add(hwTimeMs);
                WriteLine($"HW accelerated addition took: {hwTimeMs}ms (last value = {ushortsOneCopy[ushortsOneCopy.Length - 1]}).");

                ushortsOne.CopyTo(ushortsOneCopy, 0);
                sw.Restart();
                NaiveSumInPlaceUnchecked(ushortsOneCopy, ushortsTwo);
                var naiveTimeMs = sw.ElapsedMilliseconds;
                naiveTimesMs.Add(naiveTimeMs);
                WriteLine($"Naive addition took:          {naiveTimeMs}ms (last value = {ushortsOneCopy[ushortsOneCopy.Length - 1]}).");
            }

            WriteLine("Testing floats");
            WriteLine($"Naive method average time:          {naiveTimesMs.Average()}");
            WriteLine($"HW accelerated method average time: {hwTimesMs.Average()}");
            WriteLine($"Hardware speedup:                   {(naiveTimesMs.Average() / hwTimesMs.Average()) * 100:0.00}%");
        }

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

        private static ushort[] GetRandomTestData(int testSetSize) => Enumerable.Range(0, testSetSize).Select(x => RandomUShort()).ToArray();
    }
}