using System.Linq;
using NUnit.Framework;
using static SimdSpike.UShortSimdProcessor;
using static SimdSpike.Utilities;

namespace SimdSpike {
    [TestFixture]
    public class UShortSimdProcessorUnitTest {
        delegate void MinMaxFunc(ushort[] input, out ushort min, out ushort max);
        delegate uint TotalOFArrayFunc(ushort[] input);
        private int hdImageSize = 3840*2160;
        private int smallTestSet = 15;

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
            ValidateMinMaxFunction(NaiveMinMax);
        }

        [Test]
        public void ShouldGetMinMaxUsingHwAcceleration() {
            ValidateMinMaxFunction(HWAcceleratedMinMax);
        }

        [Test]
        public void ShouldGetTotalOfArrayUsingNaiveMethod() {
            ValidateTotalOfArrayFunction(NaiveTotalOfArray);
        }

        [Test]
        public void ShouldGetTotalOfArrayUsingHWAcceleratedMethod() {
            ValidateTotalOfArrayFunction(HWAcceleratedTotalOfArray);
        }

        private void ValidateTotalOfArrayFunction(TotalOFArrayFunc totalFunc) {
            foreach (var testSetSize in new[] { smallTestSet, hdImageSize }) {
                var testDataSet = GetRandomUShortArray(testSetSize);
                var expectedTotal = testDataSet.Aggregate<ushort, uint>(0, (current, value) => current + value);

                var calculatedTotal = totalFunc(testDataSet);

                Assert.AreEqual(expectedTotal, calculatedTotal, $"Failed to calculate total for {nameof(smallTestSet)}");
            }
        }

        private void ValidateMinMaxFunction(MinMaxFunc minMaxFunc) {
            foreach (var testSetSize in new[] {smallTestSet, hdImageSize}) {
                var testDataSet = GetRandomUShortArray(testSetSize);
                var expectedMin = testDataSet.Min();
                var expectedMax = testDataSet.Max();

                ushort calculatedMin, calculatedMax;
                minMaxFunc(testDataSet, out calculatedMin, out calculatedMax);

                Assert.AreEqual(expectedMin, calculatedMin, $"Failed to calculate mininum for {nameof(smallTestSet)}");
                Assert.AreEqual(expectedMax, calculatedMax, $"Failed to calculate maximum for {nameof(smallTestSet)}");
            }
        }
    }
}