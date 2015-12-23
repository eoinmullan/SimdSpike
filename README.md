# SimdSpike

.NET 4.6 is shipped with a new 64-bit JIT comipler, [RyuJIT](https://github.com/dotnet/coreclr/tree/master/src/jit), the primary goal of which is to speed up jitting of 64-bit applications. The new JIT comiler also enables hardware acceleration of .NET apps via the use of [SIMD](https://en.wikipedia.org/wiki/SIMD) instructions.

This C# spike investigates the performance gains that can be achieved using SIMD capabilities, as well as some common SIMD patterns.
