using NUnit.Framework;

namespace SimdSpike {
    [TestFixture]
    public class UShortSimdProcessorUnitTest {
        [Test]
        public void ShouldAddFloatArraysIntoProvidedArray() {
            TestHelper.ValidateAdditionMethodsUnchecked(UShortSimdProcessor.NaiveSumUnchecked);
        }

        [Test]
        public void ShouldAddFloatArraysInPlace() {
            TestHelper.ValidateAdditionInPlaceMethodsUnchecked(UShortSimdProcessor.NaiveSumInPlaceUnchecked, UShortSimdProcessor.HwAcceleratedSumInPlaceUnchecked);
        }

        [Test]
        public void ShouldAddFloatArraysWithFuncs() {
            TestHelper.ValidateAdditionFuncsUnchecked(UShortSimdProcessor.NaiveSumFuncUnchecked);
        }
    }
}