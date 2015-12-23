using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Compatibility;
using static System.Console;
using static SimdSpike.FloatSimdProcessor;
using static SimdSpike.UShortSimdProcessor;
using static SimdSpike.Utilities;

namespace SimdSpike {
    public class PerformanceTests {
        public static void TestInPlaceFloatAddition(int testSetSize) {
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

        public static void TestInPlaceUShortAddition(int testSetSize) {
            WriteLine();
            Write("Testing floats, generating test data...");
            var ushortsOne = GetRandomUShortArray(testSetSize);
            var ushortsTwo = GetRandomUShortArray(testSetSize);
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
    }
}