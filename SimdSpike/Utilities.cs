using System;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
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

        internal static float[] GetRandomFloatArray(int testSetSize) => Enumerable.Range(0, testSetSize).Select(x => RandomFloat()).ToArray();

        internal static ushort[] GetRandomUShortArray(int testSetSize) => Enumerable.Range(0, testSetSize).Select(x => RandomUShort()).ToArray();

        internal static int[] GetRandomIntArray(int testSetSize) => Enumerable.Range(0, testSetSize).Select(x => RandomInt()).ToArray();

        public static void PrintHardwareSpecificSimdEffectiveness() {
            WriteLine($"Simd register is {Vector<int>.Count * sizeof(int)} bytes");
            try { WriteLine($"{Vector<byte>.Count} bytes ({sizeof(byte)} bytes each) per operation"); } catch { WriteLine($"Simd not supported for byte"); }
            try { WriteLine($"{Vector<char>.Count} chars ({sizeof(char)} bytes each) per operation"); } catch { WriteLine($"Simd not supported for char"); }
            try { WriteLine($"{Vector<float>.Count} floats ({sizeof(float)} bytes each) per operation"); } catch { WriteLine($"Simd not supported for float"); }
            try { WriteLine($"{Vector<short>.Count} shorts ({sizeof(short)} bytes each) per operation"); } catch { WriteLine($"Simd not supported for short"); }
            try { WriteLine($"{Vector<ushort>.Count} ushorts ({sizeof(ushort)} bytes each) per operation"); } catch { WriteLine($"Simd not supported for ushort"); }
            try { WriteLine($"{Vector<int>.Count} ints ({sizeof(int)} bytes each) per operation"); } catch { WriteLine($"Simd not supported for int"); }
            try { WriteLine($"{Vector<uint>.Count} uints ({sizeof(uint)} bytes each) per operation"); } catch { WriteLine($"Simd not supported for uint"); }
            try { WriteLine($"{Vector<long>.Count} longs ({sizeof(long)} bytes each) per operation"); } catch { WriteLine($"Simd not supported for long"); }
            try { WriteLine($"{Vector<ulong>.Count} ulongs ({sizeof(ulong)} bytes each) per operation"); } catch { WriteLine($"Simd not supported for ulong"); }
            try { WriteLine($"{Vector<double>.Count} doubles ({sizeof(double)} bytes each) per operation"); } catch { WriteLine($"Simd not supported for double"); }
            try { WriteLine($"{Vector<decimal>.Count} decimals ({sizeof(decimal)} bytes each) per operation"); } catch { WriteLine($"Simd not supported for decimal"); }
        }
    }
}