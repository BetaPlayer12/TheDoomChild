using DChild.Gameplay.Characters.Enemies;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerController : MonoBehaviour
{
    [SerializeField]
    private TeleportPlayer m_teleportPlayerScript;
    [SerializeField]
    private CelestialVoidAI m_celestialVoidAI;
    public Collider2D m_boxCollider;

    private float time = 0f;
    private float duration = 1.5f;
    private bool willTeleport;
    private bool lastBoundCheckRequestResult;
    public Vector2 m_playerPos;

    public EventAction<EventActionArgs> OnPlayerStay;

    public bool IsTargetInsideBounds(Vector3 targetPosition)
    {
        var bounds = m_boxCollider.bounds;
        var minBounds = bounds.min;
        var maxBounds = bounds.max;
        bool isWithinXBounds = targetPosition.x >= minBounds.x && targetPosition.x <= maxBounds.x;
        bool isWithinYBounds = targetPosition.y >= minBounds.y && targetPosition.y <= maxBounds.y;

        var isInsideBounds = isWithinXBounds && isWithinYBounds;
        lastBoundCheckRequestResult = isInsideBounds;
        return isInsideBounds;
    }

    /*private IEnumerator TurnOffRoutine()
    {
        yield return new WaitForSeconds(.5f);
        m_boxCollider.enabled = false;
    }*/
    public bool m_checker;
    public IEnumerator TriggerRoutine()
    {
        m_checker = true;
        time = 0f;
        duration = 1f;
        while (duration > time)
        {
            time += Time.deltaTime;
            yield return null;
        }
        if (time >= duration)
        {
            Debug.Log("done");
            m_boxCollider.enabled = false;
            willTeleport = true;
            m_checker = false;
            EventSender();
        }
        yield return null;
    }
    public Coroutine TeleportRoutine;
    public void EventSender()
    {
        if (willTeleport)
        {
            OnPlayerStay?.Invoke(this, EventActionArgs.Empty);
            TeleportRoutine = StartCoroutine(m_celestialVoidAI.ActivateTeleportFX());
            willTeleport = false;
        }
    }

    //    var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
    //    if (playerObject != null && collision.tag != "Sensor" && playerObject.owner == (IPlayer)GameplaySystem.playerManager.player)
    //    {
    //        if (collision.tag == "Hitbox")
    //        {
    //            StartCoroutine(TriggerRoutine());
    //        }
    //    }
    //}
    // Start is called before the first frame update
    /*void Start()
    {
        
    }*/

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
