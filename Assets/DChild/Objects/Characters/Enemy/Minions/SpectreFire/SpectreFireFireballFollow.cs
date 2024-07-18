using System;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Spine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DChild;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Pathfinding;

public class SpectreFireFireballFollow : MonoBehaviour
{

    [SerializeField]
    private PathFinderAgent m_movement;
    [SerializeField]
    private GameObject m_dissipateState;
    public GameObject m_player;
    public GameObject m_spectreFire;

    private bool m_willDissipate;

    public EventAction<EventActionArgs> OnFireballDissipate;

    private IEnumerator FollowPlayer()
    {
        Vector2 targetPoint = m_player.transform.position;
        while (Vector2.Distance(transform.position, targetPoint) > 10f && !m_willDissipate)
        {
            DynamicMovement(m_player.transform.position, 10);
            yield return null;
        }
        m_movement.Stop();
        yield return null;
    }
    private IEnumerator TimerRoutine()
    {
        var duration = 10f;
        var timeLeft = 0f;
        while(duration > timeLeft)
        {
            timeLeft += Time.deltaTime;
            if(timeLeft >= duration)
            {
                timeLeft = 0;
                m_willDissipate = true;
                m_movement.Stop();
                Instantiate(m_dissipateState, transform.position, Quaternion.identity);
                OnFireballDissipate?.Invoke(this, EventActionArgs.Empty);
                gameObject.SetActive(false);
            }
            yield return null;
        }
        yield return null;
    }
    private void DynamicMovement(Vector2 target, float moveSpeed)
    {
        m_movement.SetDestination(target);
        m_movement.Move(moveSpeed);
    }
    void Start()
    {
        StartCoroutine(TimerRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(FollowPlayer());
    }
}
