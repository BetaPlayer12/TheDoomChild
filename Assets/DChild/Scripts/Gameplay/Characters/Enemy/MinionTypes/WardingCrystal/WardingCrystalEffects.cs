using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WardingCrystalEffects 
{
    [SerializeField]
    private IncreaseHealthMinion m_increaseHealthMinion;
    [SerializeField]
    private IncreaseDamageMinion m_increaseDamageMinion;
    [SerializeField]
    private SoulEssenceDropper m_soulEssenceDropper;
    // Start is called before the first frame update
   

    public void AddtionalAttackAndHealthBuff(GameObject minion)
    {
        m_increaseHealthMinion.AddHealth(minion);
        m_increaseDamageMinion.MultiplyMinionDamage(minion);
    }
    
    public void SoulEssenceBuff(List<GameObject> minions)
    {
        m_soulEssenceDropper.SoulEssenceCoroutine(minions);
    }

    public void ShowVFX(GameObject minion)
    {
        m_increaseHealthMinion.ShowHealVFX(minion);
        m_increaseDamageMinion.ShowIncreaseDamageVFX(minion);
    }

    public void RemoveVFX(GameObject minion)
    {
        m_increaseHealthMinion.RemoveHealVFX(minion);
        m_increaseDamageMinion.RemoveIncreaseDamageVFX(minion);
    }
    
    public void ReturnBaseDamageMinion(GameObject minion)
    {
        m_increaseDamageMinion.ResetBaseDamage(minion);
    }
    public void SetStatusEssenceDrop(bool value)
    {
        m_soulEssenceDropper.SetBuffValue(value);
    }
    public void MinionCount(bool value, List<GameObject> minions)
    {
        m_soulEssenceDropper.MinionChecker(value, minions);
    }
    public void ReturnBaseHealthMinion(GameObject minion)
    {
        m_increaseHealthMinion.ReturnToUnbuffedHealth(minion);
    }
}
