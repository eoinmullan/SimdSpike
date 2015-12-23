using System.Linq;
using NUnit.Framework;
using static SimdSpike.UShortSimdProcessor;
using static SimdSpike.Utilities;

namespace SimdSpike {
    [TestFixture]
    public class UShortSimdProcessorUnitTest {
        delegate void MinMaxFunc(ushort[] input, out ushort min, out ushort max);

        private int testSetSize = 3840*2160;

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

        [Test]
        public void ShouldGetMinMaxUsingNaiveMethod() {
            ValidateMinMaxFunction(NaiveMaxMin);
        }

        [Test]
        public void ShouldGetMinMaxUsingHwAcceleration() {
            ValidateMinMaxFunction(HWAcceleratedMaxMin);
        }

        private void ValidateMinMaxFunction(MinMaxFunc minMaxFunc) {
            var testDataSet = GetRandomUShortArray(testSetSize);
            var expectedMin = testDataSet.Min();
            var expectedMax = testDataSet.Max();

            ushort calculatedMin, calculatedMax;
            minMaxFunc(testDataSet, out calculatedMin, out calculatedMax);

            Assert.AreEqual(expectedMin, calculatedMin);
            Assert.AreEqual(expectedMax, calculatedMax);
        }
    }
}