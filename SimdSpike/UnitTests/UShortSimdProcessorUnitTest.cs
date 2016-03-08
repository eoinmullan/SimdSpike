using System;
using System.Linq;
using NUnit.Framework;
using static SimdSpike.UShortSimdProcessor;
using static SimdSpike.Utilities;

namespace SimdSpike {
    [TestFixture]
    public class UShortSimdProcessorUnitTest {
        delegate void MinMaxFunc(ushort[] input, out ushort min, out ushort max);
        delegate ulong TotalOFArrayFunc(ushort[] input);
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

        [Test]
        public void ShouldGetUncheckedTotalOfArrayUsingNaiveAndHWAcceleratedMethod() {
            foreach (var testSetSize in new[] {smallTestSet, hdImageSize}) {
                var testDataSet = GetRandomUShortArray(testSetSize);
                Assert.AreEqual(NaiveUncheckedTotalOfArray(testDataSet), HWAcceleratedUncheckedTotalOfArray(testDataSet));
            }
        }

        [Test]
        public void ShouldAddUShortArraysIntoProvidedArray() {
            TestHelper.ValidateAdditionMethodsUnchecked(HwAcceleratedSumUnchecked);
        }

        [Test]
        public void ShouldGetStats() {
            foreach (var testSetSize in new[] {smallTestSet, hdImageSize}) {
                var testDataSet = GetRandomUShortArray(testSetSize);
                ushort min, max;
                double average;

                var expectedMin = testDataSet.Min();
                var expectedMax = testDataSet.Max();
                var expectedAverage = testDataSet.Select(x => (int)x).Average();

                NaiveGetStats(testDataSet, out min, out max, out average);
                Assert.AreEqual(expectedMin, min, "Naive Min incorrect");
                Assert.AreEqual(expectedMax, max, "Naive Max incorrect");
                Assert.AreEqual(expectedAverage, average, "Naive Average incorrect");

                HWAcceleratedGetStats(testDataSet, out min, out max, out average);
                Assert.AreEqual(expectedMin, min, "HW Min incorrect");
                Assert.AreEqual(expectedMax, max, "HW Max incorrect");
                Assert.AreEqual(expectedAverage, average, "HW Average incorrect");
            }
        }

        private void ValidateTotalOfArrayFunction(TotalOFArrayFunc totalFunc) {
            foreach (var testSetSize in new[] { smallTestSet, hdImageSize }) {
                var testDataSet = GetRandomUShortArray(testSetSize);
                var expectedTotal = testDataSet.Aggregate<ushort, ulong>(0, (current, value) => current + value);

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