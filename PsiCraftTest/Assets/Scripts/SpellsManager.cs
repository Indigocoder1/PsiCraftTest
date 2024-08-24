using System.Collections.Generic;
using UnityEngine;

public class SpellsManager : MonoBehaviour
{
    [SerializeField] private List<SpellBehavior> spellBehaviors;

    public List<SpellBehavior> GetSpellBehaviors()
    {
        return spellBehaviors;
    }
}
