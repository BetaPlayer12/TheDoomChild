using DChild.Gameplay;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDrain : MonoBehaviour
{
    
    [SerializeField]
    private Damage m_damagePerInterval;
    [SerializeField]
    private Damage m_damagePerEnragedInterval;
    [SerializeField]
    private int m_heal;
    [SerializeField]
    private int m_enragedHeal;
    [SerializeField]
    private int m_postRageHeal;
    private IDamageable[] m_damageable;
    [SerializeField]
    private IHealable[] m_healable;
    [SerializeField, MinValue(0.1)]
    private float m_drainDelay = 0.2f;
    [SerializeField]
    private CelestialVoidAI m_celestialVoidAI;

    private bool m_drainable = false;
    private bool m_enraged = false;

    public void setrage(bool value)
    {
        m_enraged = value;
    }
    public void ActivateLifeDrain()
    {
       
        this.gameObject.GetComponent<Collider2D>().enabled = true;

    }
    public void AfterRage()
    {
        GameplaySystem.combatManager.Heal(m_healable[0], m_postRageHeal);
    }
        private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.tag != "Sensor" )
        {

            m_damageable = collision.gameObject.GetComponentsInParent<IDamageable>();
            m_healable = this.gameObject.GetComponentsInParent<IHealable>();
            m_drainable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        m_drainable = false;
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        m_celestialVoidAI.DeActivateLifeDrain();
    }
        IEnumerator DelayCoroutine()
    {
        m_drainable = false;
        if (m_enraged == true)
        {
            GameplaySystem.combatManager.Damage(m_damageable[0], m_damagePerEnragedInterval);
            GameplaySystem.combatManager.Heal(m_healable[0], m_enragedHeal);
        }
        else
        {
            GameplaySystem.combatManager.Damage(m_damageable[0], m_damagePerInterval);
            GameplaySystem.combatManager.Heal(m_healable[0], m_heal);
        }
        yield return new WaitForSeconds(m_drainDelay);
        m_drainable = true;

    }
    private void FixedUpdate()
    {
        if(m_drainable == true)
        {
            StartCoroutine(DelayCoroutine());
           
        }
    }
  }
