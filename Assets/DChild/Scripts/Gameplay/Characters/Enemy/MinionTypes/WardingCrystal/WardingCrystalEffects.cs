using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardingCrystalEffects : MonoBehaviour
{
    [SerializeField]
    private IncreaseHealthMinion m_increaseHealthMinion;
    [SerializeField]
    private IncreaseDamageMinion m_increaseDamageMinion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddtionalAttackAndHealthBuff(GameObject minion)
    {
        m_increaseHealthMinion.AddHealth(minion);
        m_increaseDamageMinion.MultiplyMinionDamage(minion);
    }

    public void ShowVFX(GameObject minion)
    {
        m_increaseHealthMinion.ShowHealVFX(minion);
        m_increaseDamageMinion.ShowIncreaseDamageVFX(minion);
    }

    public void RemoveVFX(GameObject minion)
    {
        m_increaseHealthMinion.RemoveHealVFX(minion);
    }
    
    public void ReturnBaseDamageMinion(GameObject minion)
    {
        m_increaseDamageMinion.ResetBaseDamage(minion);
    }
}
