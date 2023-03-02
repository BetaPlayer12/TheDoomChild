using DChild.Gameplay;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDrain : MonoBehaviour
{

    [SerializeField]
    private Damage m_damagePerInterval;
    private IDamageable[] m_damageable;
    [SerializeField, MinValue(0.1)]
    private float m_drainDelay = 0.2f;
    [SerializeField]
    private CorpseWeaverAI m_corpseWeaverAI;
    [SerializeField]
    private BiteTriggerDetector m_detector;
    [SerializeField]
    private bool m_isTrap = false;

    private bool m_drainable = false;
 

    
    public void ActivateShadowDrain()
    {

        this.gameObject.GetComponent<Collider2D>().enabled = true;

    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag != "Sensor")
        {
            m_damageable = collision.gameObject.GetComponentsInParent<IDamageable>();
            m_drainable = true;
            if(m_isTrap == true)
            {
                m_detector.ActivateBite();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        StopAllCoroutines();
        m_drainable = false;
        if (m_isTrap == false)
        {
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            m_corpseWeaverAI.DeActivateShadowDrain();
        }
        else
        {
            m_detector.DeactivateBite();
        }
       
    }
    IEnumerator DelayCoroutine()
    {
        m_drainable = false;
        GameplaySystem.combatManager.Damage(m_damageable[0], m_damagePerInterval);
        yield return new WaitForSeconds(m_drainDelay);
        m_drainable = true;

    }
    private void FixedUpdate()
    {
        if (m_drainable == true)
        {
            StartCoroutine(DelayCoroutine());

        }
    }
}

