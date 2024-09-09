using DChild.Gameplay;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    
    /*[SerializeField]
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
    private float m_drainDelay = 0.2f;*/
    [SerializeField]
    private CelestialVoidAI m_celestialVoidAI;
    [SerializeField]
    private BoxCollider2D m_boxCollider;

    public EventAction<EventActionArgs> OnPlayerStay;

    private void Start()
    {
        m_celestialVoidAI.GetComponent<CelestialVoidAI>().OnAttackingState += OnAttackingState;
    }

    private void OnAttackingState(object sender, EventActionArgs eventArgs)
    {
        m_boxCollider.enabled = true;
        //throw new System.NotImplementedException();
    }
    private void OnDoneAttackingState(object sender, EventActionArgs eventArgs)
    {
        m_boxCollider.enabled = false;
        //Destroy(this.gameObject);
        //throw new System.NotImplementedException();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
        if (playerObject != null && collision.tag != "Sensor" && playerObject.owner == (IPlayer)GameplaySystem.playerManager.player)
        {
            if (collision.tag == "Hitbox")
            {
                StartCoroutine(TriggerRoutine());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        StopAllCoroutines();
        StartCoroutine(TurnOffRoutine());
    }
    private float time = 0f;
    private float duration = 1.5f;
    private bool willTeleport;
    private IEnumerator TurnOffRoutine()
    {
        yield return new WaitForSeconds(.5f);
        m_boxCollider.enabled = false;
    }
    private IEnumerator TriggerRoutine()
    {
        time = 0f;
        duration = 1f;
        while (duration > time)
        {
            time += Time.deltaTime;
            yield return null;
        }
        if(time >= duration)
        {
            m_boxCollider.enabled = false;
            willTeleport = true;
            EventSender();
        }
        yield return null;
    }
    public void EventSender()
    {
        if(willTeleport)
        {
            OnPlayerStay?.Invoke(this, EventActionArgs.Empty);
            StartCoroutine(m_celestialVoidAI.ActivateTeleportFX());
            willTeleport = false;
        }
    }
        /*private bool m_drainable = false;
        private bool m_enraged = false;*/

        /*public void setrage(bool value)
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

            StopAllCoroutines();
            m_drainable = false;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            //m_celestialVoidAI.DeActivateLifeDrain();
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
        }*/
    }
