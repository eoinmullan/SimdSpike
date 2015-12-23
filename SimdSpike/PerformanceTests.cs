using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Compatibility;
using static System.Console;
using static SimdSpike.FloatSimdProcessor;
using static SimdSpike.UShortSimdProcessor;
using static SimdSpike.Utilities;

namespace SimdSpike {
    public class PerformanceTests {
        private static readonly Stopwatch stopwatch = new Stopwatch();

        public static void TestInPlaceFloatAddition(int testSetSize) {
            WriteLine();
            Write("Testing floats, generating test data...");
            var floatsOne = Enumerable.Range(0, testSetSize).Select(x => RandomFloat()).ToArray();
            var floatsTwo = Enumerable.Range(0, testSetSize).Select(x => RandomFloat()).ToArray();
            WriteLine(" done, testing...");
            
            var naiveTimesMs = new List<long>();
            var hwTimesMs = new List<long>();
            for (var i = 0; i < 3; i++) {
                var floatsOneCopy = new float[floatsOne.Length];

                floatsOne.CopyTo(floatsOneCopy, 0);
                stopwatch.Restart();
                HwAcceleratedSumInPlace(floatsOneCopy, floatsTwo);
                var hwTimeMs = stopwatch.ElapsedMilliseconds;
                hwTimesMs.Add(hwTimeMs);
                WriteLine($"HW accelerated addition took: {hwTimeMs}ms (last value = {floatsOneCopy[floatsOneCopy.Length - 1]}).");

                floatsOne.CopyTo(floatsOneCopy, 0);
                stopwatch.Restart();
                NaiveSumInPlace(floatsOneCopy, floatsTwo);
                var naiveTimeMs = stopwatch.ElapsedMilliseconds;
                naiveTimesMs.Add(naiveTimeMs);
                WriteLine($"Naive addition took:          {naiveTimeMs}ms (last value = {floatsOneCopy[floatsOneCopy.Length - 1]}).");
            }

            WriteLine("Testing float array addition");
            WriteLine($"Naive method average time:          {naiveTimesMs.Average()}");
            WriteLine($"HW accelerated method average time: {hwTimesMs.Average()}");
            WriteLine($"Hardware speedup:                   {naiveTimesMs.Average() / hwTimesMs.Average():P}%");
        }

        public static void TestInPlaceUShortAddition(int testSetSize) {
            WriteLine();
            Write("Testing floats, generating test data...");
            var ushortsOne = GetRandomUShortArray(testSetSize);
            var ushortsTwo = GetRandomUShortArray(testSetSize);
            WriteLine(" done, testing...");
            
            var naiveTimesMs = new List<long>();
            var hwTimesMs = new List<long>();
            for (var i = 0; i < 3; i++) {
                var ushortsOneCopy = new ushort[ushortsOne.Length];

                ushortsOne.CopyTo(ushortsOneCopy, 0);
                stopwatch.Restart();
                HwAcceleratedSumInPlaceUnchecked(ushortsOneCopy, ushortsTwo);
                var hwTimeMs = stopwatch.ElapsedMilliseconds;
                hwTimesMs.Add(hwTimeMs);
                WriteLine($"HW accelerated addition took: {hwTimeMs}ms (last value = {ushortsOneCopy[ushortsOneCopy.Length - 1]}).");

                ushortsOne.CopyTo(ushortsOneCopy, 0);
                stopwatch.Restart();
                NaiveSumInPlaceUnchecked(ushortsOneCopy, ushortsTwo);
                var naiveTimeMs = stopwatch.ElapsedMilliseconds;
                naiveTimesMs.Add(naiveTimeMs);
                WriteLine($"Naive addition took:          {naiveTimeMs}ms (last value = {ushortsOneCopy[ushortsOneCopy.Length - 1]}).");
            }

            WriteLine("Testing ushort array addition");
            WriteLine($"Naive method average time:          {naiveTimesMs.Average()}");
            WriteLine($"HW accelerated method average time: {hwTimesMs.Average()}");
            WriteLine($"Hardware speedup:                   {naiveTimesMs.Average() / hwTimesMs.Average():P}%");
        }
    }
}