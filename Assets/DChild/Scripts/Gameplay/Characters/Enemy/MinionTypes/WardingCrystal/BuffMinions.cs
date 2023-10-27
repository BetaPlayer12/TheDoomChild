using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild.Gameplay;


public class BuffMinions : MonoBehaviour
{
    [SerializeField]
    private WardingCrystalEffects m_wardingCrystalEffects;
    
    [SerializeField]
    private int m_minionLayer;
    [SerializeField]
    private string m_minionTag;
    [SerializeField]
    private List<GameObject> m_minionsInWardingRadius;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == m_minionLayer &&
            collision.gameObject.CompareTag(m_minionTag))
        {
            var minion = collision.gameObject;
            m_minionsInWardingRadius.Add(minion);
            m_wardingCrystalEffects.SetStatusEssenceDrop(true);
            m_wardingCrystalEffects.AddtionalAttackAndHealthBuff(minion);
            m_wardingCrystalEffects.ShowVFX(minion);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == m_minionLayer &&
             collision.gameObject.CompareTag(m_minionTag))
        {
            var minion = collision.gameObject;
            m_wardingCrystalEffects.SoulEssenceBuff(m_minionsInWardingRadius);
            m_minionsInWardingRadius.Remove(minion);
            m_wardingCrystalEffects.RemoveVFX(minion);
            m_wardingCrystalEffects.MinionCount(false, m_minionsInWardingRadius);
            m_wardingCrystalEffects.ReturnBaseDamageMinion(minion);
        }


    }
}
