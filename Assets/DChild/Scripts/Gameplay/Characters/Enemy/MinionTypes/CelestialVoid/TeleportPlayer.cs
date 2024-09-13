using DChild.Gameplay;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("DChild/Gameplay/Enemies/Minion/TriggerForTeleportationCounter")]
public class TeleportPlayer : MonoBehaviour
{
    [SerializeField]
    private CelestialVoidAI m_celestialVoidAI;
    [SerializeField]
    private TeleportPlayerController m_teleportPlayerController;
    [SerializeField]
    private BoxCollider2D m_boxCollider;
    [SerializeField]
    private bool boolenitoto;

    public EventAction<EventActionArgs> OnPlayerStay;

    /*public bool IsPlayerInsideBounds()
    {
        *//*if (m_boxCollider.bounds.Contains(m_celestialVoidAI.m_playerPos))
        {
            Debug.Log("sulod di sa");
            return true;
        }
        else
        {
            StartCoroutine(TurnOffRoutine());
            Debug.Log("player pos" + m_celestialVoidAI.m_playerPos);
            return false;
        }*//*
        return false;
    }*/

    /*private void Start()
    {
        //m_celestialVoidAI.GetComponent<CelestialVoidAI>().OnAttackingState += OnAttackingState;
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
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hitbox")
        {
            m_teleportPlayerController.m_boxCollider.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        m_teleportPlayerController.m_boxCollider.enabled = false;
    }
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
    //    if (playerObject != null && collision.tag != "Sensor" && playerObject.owner == (IPlayer)GameplaySystem.playerManager.player)
    //    {
    //        if (collision.tag == "Hitbox")
    //        {
    //            StartCoroutine(TriggerRoutine());
    //        }
    //    }
    //}

    private float time = 0f;
    private float duration = 1.5f;
    private bool willTeleport;
    private bool lastBoundCheckRequestResult;
    public Vector2 m_playerPos;

    public bool IsTargetInsideBounds(Vector3 targetPosition) {
       var isInsideBounds = m_boxCollider.bounds.Contains(targetPosition);
        lastBoundCheckRequestResult = isInsideBounds;
        return isInsideBounds;
    }

    /*private IEnumerator TurnOffRoutine()
    {
        yield return new WaitForSeconds(.5f);
    }*/
    /*public IEnumerator TriggerRoutine()
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
    }*/
    /*public void EventSender()
    {
        if(willTeleport)
        {
            OnPlayerStay?.Invoke(this, EventActionArgs.Empty);
            StartCoroutine(m_celestialVoidAI.ActivateTeleportFX());
            willTeleport = false;
        }
    }*/
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
/*    private void Update()
    {
        boolenitoto = 
        if (boolenitoto)
        {
            Debug.Log("true");
        }
        Debug.Log("sulod di sa" + m_boxCollider.bounds.center);
        Debug.Log("player ni sa" + m_playerPos);
    }*/
}
