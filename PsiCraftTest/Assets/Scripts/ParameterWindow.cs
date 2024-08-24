using System;
using System.Reflection;
using UnityEngine;

public class ParameterWindow : MonoBehaviour
{
    public static ParameterWindow Instance { get; private set; }

    [SerializeField] private GameObject parameterPrefab;
    [SerializeField] private Transform parametersParent;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void SetTargetMethod(MethodInfo methodInfo, SpellItem spellItem)
    {
        foreach (Transform child in parametersParent)
        {
            DestroyImmediate(child.gameObject);
        }

        foreach (var parameterInfo in methodInfo.GetParameters())
        {
            var item = Instantiate(parameterPrefab, parametersParent).GetComponent<ParameterItem>();

            item.SetParameter(parameterInfo.Name, parameterInfo.ParameterType, parameterInfo.DefaultValue != DBNull.Value, spellItem);
        }

        foreach (var item in parametersParent.GetComponentsInChildren<ParameterItem>())
        {
            item.SetSelectedSelector();
        }
    }
}
