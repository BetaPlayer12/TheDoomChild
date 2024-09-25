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
    [SerializeField]
    private List<GameObject> m_checkedMinions;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == m_minionLayer /*&&
            collision.gameObject.CompareTag(m_minionTag)*/)
        {
            var minionAttacker = collision.GetComponentInParent<Attacker>();
            if (minionAttacker != null)
            {
                //Minion is already recorded in Radius
                var minion = minionAttacker.gameObject;
                if (m_minionsInWardingRadius.Contains(minion))
                {
                    return;
                }

                //Minions Just got into Radius
                m_wardingCrystalEffects.AddtionalAttackAndHealthBuff(minion);
                m_wardingCrystalEffects.ShowVFX(minion);

                m_minionsInWardingRadius.Add(minion);
            }
            m_wardingCrystalEffects.SetStatusEssenceDrop(true);
            //for (int i = 0; i < m_minionsInWardingRadius.Count; i++)
            //{
                
            //    var minionToAdd = m_minionsInWardingRadius[i];
            //    var minionHasBuf = m_minionsInWardingRadius[i].transform.parent.GetComponentInChildren<PoolableObject>();
            //    if (!minionHasBuf)
            //    {
            //        m_checkedMinions.Add(minionToAdd);
            //    }
            //}

            //foreach (var obj in m_checkedMinions)
            //{
            //    var ifChildHaveBuff = obj.GetComponentInChildren<PoolableObject>();
            //    if (!ifChildHaveBuff)
            //    {
            //        m_wardingCrystalEffects.AddtionalAttackAndHealthBuff(obj);
            //        m_wardingCrystalEffects.ShowVFX(obj);
            //    }
            //}
        
            //m_minionsInWardingRadius.Add(minion);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == m_minionLayer /*&&
             collision.gameObject.CompareTag(m_minionTag)*/)
        {

            var minionAttacker = collision.GetComponentInParent<Attacker>();
            if (minionAttacker != null)
            {
                //Minion is already recorded in Radius
                var minion = minionAttacker.gameObject;
                if (m_minionsInWardingRadius.Contains(minion))
                {
                    m_wardingCrystalEffects.SoulEssenceBuff(m_minionsInWardingRadius);
                    m_wardingCrystalEffects.RemoveVFX(minion);
                    m_wardingCrystalEffects.ReturnBaseDamageMinion(minion);
                    m_wardingCrystalEffects.ReturnBaseHealthMinion(minion);
                    m_wardingCrystalEffects.MinionCount(false, m_minionsInWardingRadius);
                    m_minionsInWardingRadius.Remove(minion);
                }
            }
        }


    }
}
