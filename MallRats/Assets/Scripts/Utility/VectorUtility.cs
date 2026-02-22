using UnityEngine;

public static class VectorUtility
{
    public static Vector3 ProjectOnPlane(Vector3 vectorToProject, Vector3 planeNormal)
    {
        return vectorToProject - planeNormal * Vector3.Dot(vectorToProject, planeNormal);
    }
}
