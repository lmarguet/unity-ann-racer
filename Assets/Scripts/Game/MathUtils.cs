using UnityEngine;

namespace Game
{
    public static class MathUtils
    {
        public static bool AreVectorEquals(Vector3 vectorA, Vector3 vectorB, float tolerance)
        {
            return Vector3.Distance(vectorA, vectorB) <= tolerance;
        }
    }
}