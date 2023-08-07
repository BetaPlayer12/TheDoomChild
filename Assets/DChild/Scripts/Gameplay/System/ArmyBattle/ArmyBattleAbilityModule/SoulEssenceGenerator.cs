using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.ArmyBattle;


public class SoulEssenceGenerator : IArmyAbilityEffect
{
    [SerializeField] 
    private int m_soulEssence;
    [SerializeField]
    private int m_minSoulEssence;
    [SerializeField]
    private int m_maxSoulEssence;
    

    public void ApplyEffect(Army owner, Army opponent)
    {
        int m_soulEssence = Random.Range(m_minSoulEssence, m_maxSoulEssence);
        Debug.Log(m_soulEssence);

    }
}
