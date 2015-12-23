using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using static System.Console;
using static SimdSpike.PerformanceTests;
using static SimdSpike.Utilities;

namespace SimdSpike {
    internal class Program {
        private static Random random = new Random();
        private static Stopwatch sw = new Stopwatch();
        private const int EightKUltraHDRes = 7680 * 4320;

        static void Main() {
            if (!Vector.IsHardwareAccelerated) {
                WriteLine("Hardware acceleration not supported.");
                return;
            }
            WriteLine("Hardware acceleration is supported");

            PrintHardwareSpecificSimdEffectiveness();
         
            TestInPlaceFloatAddition(EightKUltraHDRes);
            TestInPlaceUShortAddition(EightKUltraHDRes);
            TestIntMaxMinFunctions(EightKUltraHDRes);
            TestUShortMinMaxFunctions(EightKUltraHDRes);
        }
    }
}
