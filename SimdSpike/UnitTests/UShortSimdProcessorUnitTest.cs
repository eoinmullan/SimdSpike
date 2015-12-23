using NUnit.Framework;

namespace SimdSpike {
    [TestFixture]
    public class UShortSimdProcessorUnitTest {
        [Test]
        public void ShouldAddFloatArraysIntoProvidedArray() {
            TestHelper.ValidateAdditionMethodsUnchecked(NaiveSumUnchecked);
        }

        [Test]
        public void ShouldAddFloatArraysInPlace() {
            TestHelper.ValidateAdditionInPlaceMethodsUnchecked(NaiveSumInPlaceUnchecked, HwAcceleratedSumInPlaceUnchecked);
        }

        [Test]
        public void ShouldAddFloatArraysWithFuncs() {
            TestHelper.ValidateAdditionFuncsUnchecked(NaiveSumFuncUnchecked);
        }
    }
}