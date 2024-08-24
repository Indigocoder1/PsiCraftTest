using UnityEngine;

public class ParameterGridOffset : MonoBehaviour
{
    [SerializeField] private Vector2Int offset;

    public Vector2Int GetOffset()
    {
        return offset;
    }
}
