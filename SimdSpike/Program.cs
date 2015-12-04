using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using NUnit.Framework;
using Stopwatch = NUnit.Framework.Compatibility.Stopwatch;

namespace SimdSpike {
    class Program {
        private static Random random = new Random();

        static void Main(string[] args) {
            var testSize = 20000000;

            var one = Enumerable.Range(0, testSize).Select(x => RandomFloat()).ToArray();
            var two = Enumerable.Range(0, testSize).Select(x => RandomFloat()).ToArray();
            Console.WriteLine("Beginning addition");
            var sw = new Stopwatch();
            for (var i = 0; i < 10; i++) {
                sw.Restart();
                var sum = SumArrays(one, two);
                Console.WriteLine($"Addition took {sw.ElapsedMilliseconds}. ({sum[testSize - 1]})");
            }

//            var one = new HarewareAcceleratedArray<float>(Enumerable.Range(0, testSize).Select(x => RandomFloat()).ToArray());
//            var two = new HarewareAcceleratedArray<float>(Enumerable.Range(0, testSize).Select(x => RandomFloat()).ToArray());
//            Console.WriteLine("Beginning addition");
//            var sw = new Stopwatch();
//            for (var i = 0; i < 10; i++) {
//                sw.Restart();
//                var sum = one + two;
//                Console.WriteLine($"Addition took {sw.ElapsedMilliseconds}. ({sum[testSize - 1]})");
//            }

//            for (int i = 0; i < testSize; i++) {
//                Assert.AreEqual(one[i] + two[i], sum[i]);
//            }
        }

        private static float[] SumArrays(float[] l, float[] r) {
            var returnArray = new float[l.Length];

            for (var i = 0; i < l.Length; i++) {
                returnArray[i] = l[i] + r[i];
            }

            return returnArray;
        }

        private static float RandomFloat() {
            double mantissa = (random.NextDouble() * 2.0) - 1.0;
            double exponent = Math.Pow(2.0, random.Next(-126, 128));
            return (float)(mantissa * exponent);
        }
    }
}
