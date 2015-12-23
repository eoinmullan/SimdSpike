using System.Linq;
using NUnit.Framework;
using static SimdSpike.IntSimdProcessor;
using static SimdSpike.Utilities;

namespace SimdSpike {
    public class IntSimdProcessorUnitTest {
        delegate void MinMaxFunc(int[] input, out int min, out int max);
        private int testSetSize = 3840 * 2160;

        [Test]
        public void ShouldGetMinMaxUsingNaiveMethod() {
            ValidateMinMaxFunction(NaiveMinMax);
        }

        [Test]
        public void ShouldGetMinMaxUsingHwAcceleration() {
            ValidateMinMaxFunction(HWAcceleratedMinMax);
        }

        private void ValidateMinMaxFunction(MinMaxFunc minMaxFunc) {
            var testDataSet = GetRandomIntArray(testSetSize);
            var expectedMin = testDataSet.Min();
            var expectedMax = testDataSet.Max();

            int calculatedMin, calculatedMax;
            minMaxFunc(testDataSet, out calculatedMin, out calculatedMax);

            Assert.AreEqual(expectedMin, calculatedMin);
            Assert.AreEqual(expectedMax, calculatedMax);
        }
    }
}