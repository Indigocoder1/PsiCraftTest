using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class ParameterWindow : MonoBehaviour
{
    public static ParameterWindow Instance { get; private set; }

    [SerializeField] private GameObject parameterPrefab;
    [SerializeField] private Transform parametersParent;

    private SpellItem currentSpellItem;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Backspace)) return;

        if (currentSpellItem == null) return;

        currentSpellItem.ClearBehavior();

        foreach (Transform child in parametersParent)
        {
            Destroy(child.gameObject);
        }

        currentSpellItem = null;
    }

    public void SetTargetMethod(MethodInfo methodInfo, SpellItem spellItem)
    {
        currentSpellItem = spellItem;

        StartCoroutine(SpawnItems(methodInfo, spellItem));
    }

    private IEnumerator SpawnItems(MethodInfo methodInfo, SpellItem spellItem)
    {
        foreach (Transform child in parametersParent)
        {
            Destroy(child.gameObject);
        }

        Dictionary<ParameterItem, ParameterInfo> items = new();

        foreach (var parameterInfo in methodInfo.GetParameters())
        {
            var item = Instantiate(parameterPrefab, parametersParent).GetComponent<ParameterItem>();
            item.SetParameter(parameterInfo.Name, parameterInfo.ParameterType, parameterInfo.DefaultValue != DBNull.Value, spellItem);
            items.Add(item, parameterInfo);
        }

        yield return new WaitForEndOfFrame();

        foreach (var item in items)
        {
            var paramItem = item.Key;
            var info = item.Value;
            
            paramItem.SetupItemDelayed();
        }
    }
}
