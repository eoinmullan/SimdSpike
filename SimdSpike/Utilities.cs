using System;
using System.Linq;
using System.Numerics;
using static System.Console;

namespace SimdSpike {
    public static class Utilities {
        private static readonly Random random = new Random();

        public static float RandomFloat() {
            var mantissa = (random.NextDouble() * 2.0) - 1.0;
            var exponent = Math.Pow(2.0, random.Next(-126, 128));
            return (float)(mantissa * exponent);
        }

        public static ushort RandomUShort() {
            return (ushort)random.Next(ushort.MinValue, ushort.MaxValue);
        }

        public static int RandomInt() {
            return random.Next(int.MinValue, int.MaxValue);
        }

        internal static ushort[] GetRandomUShortArray(int testSetSize) => Enumerable.Range(0, testSetSize).Select(x => RandomUShort()).ToArray();

        internal static int[] GetRandomIntArray(int testSetSize) => Enumerable.Range(0, testSetSize).Select(x => RandomInt()).ToArray();

        public static void PrintHardwareSpecificSimdEffectiveness() {
            WriteLine($"Simd register is {Vector<int>.Count * sizeof(int)} bytes");
            try { WriteLine($"{Vector<byte>.Count} bytes per operation"); } catch { WriteLine($"Simd not supported for byte"); }
            try { WriteLine($"{Vector<char>.Count} chars per operation"); } catch { WriteLine($"Simd not supported for char"); }
            try { WriteLine($"{Vector<float>.Count} floats per operation"); } catch { WriteLine($"Simd not supported for float"); }
            try { WriteLine($"{Vector<short>.Count} shorts per operation"); } catch { WriteLine($"Simd not supported for short"); }
            try { WriteLine($"{Vector<ushort>.Count} ushorts per operation"); } catch { WriteLine($"Simd not supported for ushort"); }
            try { WriteLine($"{Vector<int>.Count} ints per operation"); } catch { WriteLine($"Simd not supported for int"); }
            try { WriteLine($"{Vector<uint>.Count} uints per operation"); } catch { WriteLine($"Simd not supported for uint"); }
            try { WriteLine($"{Vector<long>.Count} longs per operation"); } catch { WriteLine($"Simd not supported for long"); }
            try { WriteLine($"{Vector<ulong>.Count} ulongs per operation"); } catch { WriteLine($"Simd not supported for ulong"); }
            try { WriteLine($"{Vector<double>.Count} doubles per operation"); } catch { WriteLine($"Simd not supported for double"); }
            try { WriteLine($"{Vector<decimal>.Count} decimals per operation"); } catch { WriteLine($"Simd not supported for decimal"); }
        }
    }
}