using System.Numerics;
using static System.Console;
using static SimdSpike.Utilities;

namespace SimdSpike {
    internal class Program {
        private const int TestSetSize = 8294400;

        static void Main() {
            if (!Vector.IsHardwareAccelerated) {
                WriteLine("Hardware acceleration not supported.");
                return;
            }
            Write("Hardware acceleration is supported");

            Write("Validating addition methods...");
            FloatSimdProcessor.ValidateAdditionMethods();
            WriteLine(" methods correct.");

            PrintSimdEffectiveness();
            
            FloatSimdProcessor.TestInPlaceAddition(TestSetSize);
            UShortSimdProcessor.TestInPlaceAddition(TestSetSize);
        }
    }
}
