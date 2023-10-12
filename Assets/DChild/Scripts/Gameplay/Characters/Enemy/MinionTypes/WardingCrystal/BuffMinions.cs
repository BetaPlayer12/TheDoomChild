using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMinions : MonoBehaviour
{
    [SerializeField]
    private WardingCrystalEffects m_wardingCrystalEffects;
    [SerializeField]
    private int m_minionLayer;
    [SerializeField]
    private string m_minionTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == m_minionLayer &&
            collision.gameObject.CompareTag(m_minionTag))
        {
            var minion = collision.gameObject;
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
            m_wardingCrystalEffects.RemoveVFX(minion);
            m_wardingCrystalEffects.ReturnBaseDamageMinion(minion);
        }


    }
}
