using DChild.Gameplay.Characters.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteTriggerDetector : MonoBehaviour
{
    [SerializeField]
    private bool m_isTrap = false;
    private CorpseWeaverAI[] m_corpseWeaverAI;
    public void ActivateBite()
    {
        m_isTrap = true;
    }
    public void DeactivateBite()
    {
        m_isTrap = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_isTrap == true)
        {
            m_corpseWeaverAI = null;
            m_corpseWeaverAI = collision.gameObject.GetComponentsInParent<CorpseWeaverAI>();
            if (m_corpseWeaverAI!=null)
            {
                m_corpseWeaverAI[0].ActivateBite();
            }
        }
    }
       
}
