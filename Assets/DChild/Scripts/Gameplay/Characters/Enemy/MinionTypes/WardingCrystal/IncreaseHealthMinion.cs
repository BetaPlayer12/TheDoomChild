using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild;
using DChild.Gameplay;

public class IncreaseHealthMinion : MonoBehaviour
{

    [SerializeField][Range(0,100)]
    private float m_additionalHealthPercentage;
    [SerializeField]
    private GameObject m_healVfx;
    [SerializeField]
    private GameObject m_LineRendererObj;
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
        
        Damageable m_minionDamageable = minion.GetComponentInParent<Damageable>();
        if(!m_minionDamageable)
        {
            return;
        }
        var minionCurrentmaxHealth = m_minionDamageable.health.maxValue;
        var minionAddHealth = GetMinionHealthPercentage(minionCurrentmaxHealth, m_additionalHealthPercentage);
        Debug.Log("ADDED "+minionAddHealth+"to "+minion.name+"'s max health");

        //minion.GetComponent<Damageable>().Heal(Mathf.RoundToInt(minionAddHealth));

        // Change for maxhealth buff instead of heal
        var newMaxHealth = minionCurrentmaxHealth + Mathf.RoundToInt(minionAddHealth);
        
        m_minionDamageable.health.SetMaxValue(newMaxHealth);
        m_minionDamageable.Heal(Mathf.RoundToInt(minionAddHealth));
    }

    public void ShowHealVFX(GameObject minion)
    {
        //GameObject x = Instantiate(LineRendererObj,transform);
        //x.GetComponent<LineConnect>().SetupLineConnect(transform.position, minion.transform);
        var minionComponent = minion.GetComponentInChildren<PoolableObject>();
        if (!minionComponent)
        {
            //var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_healVfx, gameObject.scene);
            //instance.SpawnAt(minion.transform.position, Quaternion.identity);
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_LineRendererObj, gameObject.scene);
            var Linerender = instance.GetComponent<LineConnect>();
            var Centermass = minion.GetComponentInParent<Character>().centerMass;
            Linerender.SetupLineConnect(Centermass, transform);
            instance.transform.SetParent(minion.transform);
        }
    }

    public void RemoveHealVFX(GameObject minion)
    {
        //minion.transform.
        var instance = minion.GetComponentInChildren<PoolableObject>();
        if (instance)
        {
            Destroy(instance.gameObject);
        }
    }

    public void ReturnToUnbuffedHealth(GameObject minion)
    {
        var minionCurrentmaxHealth = minion.GetComponentInParent<Damageable>().health.maxValue;
        var OriginalMaxHealth = UnbuffHealth(minionCurrentmaxHealth, m_additionalHealthPercentage);
        minion.GetComponentInParent<Damageable>().health.SetMaxValue(Mathf.RoundToInt(OriginalMaxHealth));
        Debug.Log("returned "+ minion.name+" to its max health of "+ OriginalMaxHealth);
    }

    private float UnbuffHealth(float Value, float additonalPercentage)
    {
        var result = Value / ((additonalPercentage/100) + 1);
        return result;
    }
}