using Game;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
    public class DriveUtilsTest
    {
        [Test]
        public void TestHitDistance()
        {
            Assert.AreEqual(DriveUtils.NormalizeHitDistance(200, 200), 0.5f);
            Assert.AreEqual(DriveUtils.NormalizeHitDistance(200, 100), 0.5f);
            Assert.AreEqual(DriveUtils.NormalizeHitDistance(200, 0), 1);
            Assert.AreEqual(DriveUtils.NormalizeHitDistance(200, 50), 1);
            Assert.AreEqual(DriveUtils.NormalizeHitDistance(200, 150), 0.5f);
        }

        [Test]
        public void TestAngleDirection()
        {
            AssetAngleDirectionCalculation(0, Vector3.left, new Vector3(-1, 0, 0), 0);
            AssetAngleDirectionCalculation(0, Vector3.right, new Vector3(1, 0, 0), 0);
            AssetAngleDirectionCalculation(0, Vector3.forward, new Vector3(0, 0, 1), 0);

            AssetAngleDirectionCalculation(45, Vector3.right, new Vector3(0.7f, 0, -0.7f), 0.05f);
            AssetAngleDirectionCalculation(-45, Vector3.right, new Vector3(0.7f, 0, 0.7f), 0.05f);

            AssetAngleDirectionCalculation(90, -Vector3.right, new Vector3(0, 0, 1), 0.1f);
        }

        private void AssetAngleDirectionCalculation(float angle, Vector3 direction, Vector3 compare, float tolerance)
        {
            AssertVectorEquality(DriveUtils.CalculateAngleDirection(angle, direction), compare, tolerance);
        }

        private static void AssertVectorEquality(Vector3 vectorA, Vector3 vectorB, float tolerance)
        {
            Assert.IsTrue(
                MathUtils.AreVectorEquals(vectorA, vectorB, tolerance)
            );
        }
    }
}