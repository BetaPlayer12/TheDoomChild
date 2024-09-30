using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild;
using DChild.Gameplay.Systems;

public class IncreaseDamageMinion : MonoBehaviour
{
    [SerializeField]
    private int m_damageMultiplier;
    [SerializeField]
    private GameObject m_IncreaseDamageVFX;

    private Damage IncreaseDamage(GameObject minion)
    {
        var minionBaseDamage = minion.GetComponent<Attacker>().GetBaseDamage();
        minionBaseDamage.value *= m_damageMultiplier;
        return minionBaseDamage;
    }
    private Damage SetBaseDamage(GameObject minion)
    {
        var minionBaseDamage = minion.GetComponent<Attacker>().GetBaseDamage();
        minionBaseDamage.value /= m_damageMultiplier; 
        return minionBaseDamage;
    }
    public void MultiplyMinionDamage(GameObject minion)
    {
        var multipliedDamage = IncreaseDamage(minion);
        minion.GetComponentInParent<Attacker>().SetDamage(multipliedDamage);
        Debug.Log(minion.name + " " + " " + minion.GetComponent<Attacker>().GetBaseDamage().value);
    }

    public void ResetBaseDamage(GameObject minion)
    {
        var baseDamage = SetBaseDamage(minion);
        minion.GetComponentInParent<Attacker>().SetDamage(baseDamage);
        Debug.Log(minion.name + " " + " " + minion.GetComponent<Attacker>().GetBaseDamage().value);
    }
    public void ShowIncreaseDamageVFX(GameObject minion)
    {
        var minionComponent = minion.GetComponentInChildren<PoolableObject>();
        if (!minionComponent)
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_IncreaseDamageVFX, gameObject.scene);
            instance.SpawnAt(minion.transform.position, Quaternion.identity);
            instance.transform.SetParent(minion.transform);
        }
    }

    public void RemoveIncreaseDamageVFX(GameObject minion)
    {
        var instance = minion.GetComponentInChildren<PoolableObject>();
        if (instance)
        {
            Destroy(instance.gameObject);
        }
       
    }
    
    
}
