using System;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SimdSpike {
    [TestFixture]
    public class HardwareAcceleratedArrayUnitTest {
        private float[] sixFloats;
        private float[] eightFloats;
        private float[] tenFloats;

        [SetUp]
        public void Setup() {
            sixFloats = new []{0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f};
            eightFloats = new []{ 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f};
            tenFloats = new []{ 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f};
        }

        [Test]
        public void ShouldConstructArray() {
            Assert.DoesNotThrow(() => new HarewareAcceleratedArray<float>(sixFloats));
            Assert.DoesNotThrow(() => new HarewareAcceleratedArray<float>(eightFloats));
            Assert.DoesNotThrow(() => new HarewareAcceleratedArray<float>(tenFloats));
        }

        [Test]
        public void ShouldConstructWithSixInputFloats() {
            var acceleratedArray = new HarewareAcceleratedArray<float>(sixFloats);

            Assert.AreEqual(sixFloats[0], acceleratedArray[0]);
            Assert.AreEqual(sixFloats[1], acceleratedArray[1]);
            Assert.AreEqual(sixFloats[2], acceleratedArray[2]);
            Assert.AreEqual(sixFloats[3], acceleratedArray[3]);
            Assert.AreEqual(sixFloats[4], acceleratedArray[4]);
            Assert.AreEqual(sixFloats[5], acceleratedArray[5]);
        }

        [Test]
        public void ShouldConstructWithEightInputFloats() {
            var acceleratedArray = new HarewareAcceleratedArray<float>(eightFloats);

            Assert.AreEqual(eightFloats[0], acceleratedArray[0]);
            Assert.AreEqual(eightFloats[1], acceleratedArray[1]);
            Assert.AreEqual(eightFloats[2], acceleratedArray[2]);
            Assert.AreEqual(eightFloats[3], acceleratedArray[3]);
            Assert.AreEqual(eightFloats[4], acceleratedArray[4]);
            Assert.AreEqual(eightFloats[5], acceleratedArray[5]);
            Assert.AreEqual(eightFloats[6], acceleratedArray[6]);
            Assert.AreEqual(eightFloats[7], acceleratedArray[7]);
        }

        [Test]
        public void ShouldConstructWithTenInputFloats() {
            var acceleratedArray = new HarewareAcceleratedArray<float>(tenFloats);

            Assert.AreEqual(tenFloats[0], acceleratedArray[0]);
            Assert.AreEqual(tenFloats[1], acceleratedArray[1]);
            Assert.AreEqual(tenFloats[2], acceleratedArray[2]);
            Assert.AreEqual(tenFloats[3], acceleratedArray[3]);
            Assert.AreEqual(tenFloats[4], acceleratedArray[4]);
            Assert.AreEqual(tenFloats[5], acceleratedArray[5]);
            Assert.AreEqual(tenFloats[6], acceleratedArray[6]);
            Assert.AreEqual(tenFloats[7], acceleratedArray[7]);
            Assert.AreEqual(tenFloats[8], acceleratedArray[8]);
            Assert.AreEqual(tenFloats[9], acceleratedArray[9]);
        }

        [Test]
        public void ShouldThrowOutOfRange() {
            var acceleratedArray = new HarewareAcceleratedArray<float>(tenFloats);

            float x;
            Assert.Throws<ArgumentOutOfRangeException>(() => x = acceleratedArray[-1]);
            Assert.Throws<ArgumentOutOfRangeException>(() => x = acceleratedArray[10]);
        }

        [Test]
        public void ShouldAddArrays() {
            var one = new HarewareAcceleratedArray<float>(tenFloats);
            var two = new HarewareAcceleratedArray<float>(tenFloats);

            var three = one + two;
            Assert.AreEqual(tenFloats[0] + tenFloats[0], three[0]);
            Assert.AreEqual(tenFloats[1] + tenFloats[1], three[1]);
            Assert.AreEqual(tenFloats[2] + tenFloats[2], three[2]);
            Assert.AreEqual(tenFloats[3] + tenFloats[3], three[3]);
            Assert.AreEqual(tenFloats[4] + tenFloats[4], three[4]);
            Assert.AreEqual(tenFloats[5] + tenFloats[5], three[5]);
            Assert.AreEqual(tenFloats[6] + tenFloats[6], three[6]);
            Assert.AreEqual(tenFloats[7] + tenFloats[7], three[7]);
            Assert.AreEqual(tenFloats[8] + tenFloats[8], three[8]);
            Assert.AreEqual(tenFloats[9] + tenFloats[9], three[9]);
        }
    }
}