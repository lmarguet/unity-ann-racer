using Game;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
    public class MathUtilsTest
    {
        [Test]
        public void TestVectorEquality()
        {
            AssertVectorEquality(new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0);
            AssertVectorEquality(new Vector3(1, 1, 1), new Vector3(1, 1, 1), 0);
            
            AssertVectorEquality(new Vector3(1, 1, 1), new Vector3(1, 1, 0), 1);
            AssertVectorEquality(new Vector3(1, 1, 1), new Vector3(1, 1, 0.01f), 1);
            
           
            AssertVectorEquality(new Vector3(1, 1, 1), new Vector3(1, 1, 0.9f), 0.11f);
        }
        
        [Test]
        public void TestVectorInequality()
        {
            AssertVectorInequality(new Vector3(0, 0, 0), new Vector3(0, 0, 1), 0);
            AssertVectorInequality(new Vector3(0, 0, 0), new Vector3(0, 0, 0.5f), 0);
            
            AssertVectorInequality(new Vector3(1, 1, 1), new Vector3(1, 1, 0.9f), 0.09f);
        }


        private static void AssertVectorEquality(Vector3 vectorA, Vector3 vectorB, float tolerance)
        {
            Assert.IsTrue(
                MathUtils.AreVectorEquals(vectorA, vectorB, tolerance)
            );
        }
        
        private static void AssertVectorInequality(Vector3 vectorA, Vector3 vectorB, float tolerance)
        {
            Assert.IsFalse(
                MathUtils.AreVectorEquals(vectorA, vectorB, tolerance)
            );
        }
    }
}