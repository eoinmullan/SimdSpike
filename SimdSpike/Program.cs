using System.Numerics;
using static System.Console;
using static SimdSpike.PerformanceTests;
using static SimdSpike.Utilities;

namespace SimdSpike {
    internal class Program {
        private const int EightKUltraHDRes = 7680 * 4320;

        static void Main() {
            if (!Vector.IsHardwareAccelerated) {
                WriteLine("Hardware acceleration not supported.");
                return;
            }
            Write("Hardware acceleration is supported");

            PrintHardwareSpecificSimdEffectiveness();
            
            TestInPlaceFloatAddition(EightKUltraHDRes);
            TestInPlaceUShortAddition(EightKUltraHDRes);
        }
    }
}
