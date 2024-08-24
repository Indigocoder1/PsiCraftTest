using System;
using UnityEngine;

public static class AllSpellBehaviors
{
    public static void DebugOutput(object input)
    {
        Debug.Log(input);
    }

    public static GameObject GetMainCamera()
    {
        return Camera.main.gameObject;
    }

    public static Vector3 GetLookDirection(GameObject gameObject)
    {
        return gameObject.transform.forward;
    }

    public static float GetConstant(float constant)
    {
        return constant;
    }

    public static Vector3 CreateVector(float x, float y, float z)
    {
        return new Vector3(x, y, z);
    }
}