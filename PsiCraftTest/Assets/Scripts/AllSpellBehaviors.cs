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
}