using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild;
using DChild.Gameplay.Systems;
public class SoulEssenceDropper : MonoBehaviour
{
    [SerializeField]
    private bool m_IsBuffed;
    public Coroutine m_soulEsscenceDropper;
    private IEnumerator SoulEsscenceDrop(List<GameObject> minions)
    {
        while (m_IsBuffed)
        {
            for (int i = 0; i < minions.Count; i++)
            {
                var minion = minions[i];
                var lootInstance = minion.GetComponentInParent<LootDropper>();
                var healthStatus = minion.GetComponentInParent<Damageable>().isAlive;
                if (!healthStatus && minions != null)
                {
                    lootInstance.DropLoot();  
                }   
            }
            yield return null;
        }
    }

    public void SetBuffValue(bool value)
    {
        m_IsBuffed = value;
    }
    public void MinionChecker(bool value, List<GameObject> minions)
    {
        if(minions.Count == 0)
        {
            SetBuffValue(value);
            Debug.Log(value);
        }
    }
    public void SoulEssenceCoroutine(List<GameObject> minions)
    {
        m_soulEsscenceDropper = StartCoroutine(SoulEsscenceDrop(minions));
    }
}
