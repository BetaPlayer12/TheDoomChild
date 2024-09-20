using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild;
public class IncreaseHealthMinion : MonoBehaviour
{

    [SerializeField]
    private float m_additionalHealthPercentage;
    [SerializeField]
    private GameObject m_healVfx;
    private void Start()
    {
    }
    private float GetMinionHealthPercentage(float minionMaxHealth, float additonalPercentage)
    {
        float result = (minionMaxHealth * additonalPercentage) / 100f;
        return result;
    }

    public void AddHealth(GameObject minion)
    {
        var minionCurrentmaxHealth = minion.GetComponent<Damageable>().health.maxValue;
        var minionAddHealth = GetMinionHealthPercentage(minionCurrentmaxHealth, m_additionalHealthPercentage);
        Debug.Log("ADDED "+minionAddHealth+"to "+minion.name+"'s max health");

        //minion.GetComponent<Damageable>().Heal(Mathf.RoundToInt(minionAddHealth));

        // Change for maxhealth buff instead of heal
        var newMaxHealth = minionCurrentmaxHealth + Mathf.RoundToInt(minionAddHealth);
        minion.GetComponent<Damageable>().health.SetMaxValue(newMaxHealth);
        minion.GetComponent<Damageable>().Heal(Mathf.RoundToInt(minionAddHealth));
    }

    public void ShowHealVFX(GameObject minion)
    {
        var minionComponent = minion.GetComponentInChildren<PoolableObject>();
        if (!minionComponent)
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_healVfx, gameObject.scene);
            instance.SpawnAt(minion.transform.position, Quaternion.identity);
            instance.transform.SetParent(minion.transform);
        }
    }

    public void RemoveHealVFX(GameObject minion)
    {
        var instance = minion.GetComponentInChildren<PoolableObject>();
        if (instance)
        {
            Destroy(instance.gameObject);
        }
    }

    public void ReturnToUnbuffedHealth(GameObject minion)
    {
        var minionCurrentmaxHealth = minion.GetComponent<Damageable>().health.maxValue;
        var OriginalMaxHealth = UnbuffHealth(minionCurrentmaxHealth, m_additionalHealthPercentage);
        minion.GetComponent<Damageable>().health.SetMaxValue(Mathf.RoundToInt(OriginalMaxHealth));
        Debug.Log("returned "+ minion.name+" to its max health of "+ OriginalMaxHealth);
    }

    private float UnbuffHealth(float Value, float additonalPercentage)
    {
        var result = Value / ((additonalPercentage/100) + 1);
        return result;
    }
}