using NUnit.Framework;

namespace SimdSpike {
    [TestFixture]
    public class FloatSimdProcessorUnitTest {
        [Test]
        public void ShouldAddFloatArraysIntoProvidedArray() {
            TestHelper.ValidateAdditionMethods(FloatSimdProcessor.NaiveSum, FloatSimdProcessor.HwAcceleratedSum);
        }

        [Test]
        public void ShouldAddFloatArraysInPlace() {
            TestHelper.ValidateAdditionInPlaceMethods(FloatSimdProcessor.NaiveSumInPlace, FloatSimdProcessor.HwAcceleratedSumInPlace);
        }

        [Test]
        public void ShouldAddFloatArraysWithFuncs() {
            TestHelper.ValidateAdditionFuncs(FloatSimdProcessor.NaiveSumFunc);
        }
    }
}