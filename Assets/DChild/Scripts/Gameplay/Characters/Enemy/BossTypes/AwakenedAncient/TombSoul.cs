using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay;
using DChild.Gameplay.Characters.AI;
using DChild;

public class TombSoul : MonoBehaviour
{
    [SerializeField]
    private float m_soulSpeed;
    [SerializeField]
    private Vector2 m_riseSpeed;
    [SerializeField]
    private float m_riseDuration;

    private float m_delay;


    private IsolatedObjectPhysics2D m_physics;
    private Collider2D m_collider;
    private AITargetInfo m_target;
    private TombSoulAnimation m_animation;
    private PhysicsMovementHandler2D m_movement;
    private bool m_willChase;

    private void Awake()
    {
        m_physics = GetComponent<IsolatedObjectPhysics2D>();
        m_animation = GetComponent<TombSoulAnimation>();
        m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedObjectPhysics2D>(), transform);
        m_collider = GetComponentInChildren<Collider2D>();
    }

    private void Start()
    {
        StartCoroutine(SoulRoutine());
    }

    public void SetAttackInfo(AITargetInfo target, float delay)
    {
        m_target = target;
        m_delay = delay;
    }

    private IEnumerator SoulRoutine()
    {
        GetComponent<Rigidbody2D>().AddForce(Random.Range(m_riseSpeed.x, m_riseSpeed.y) * Vector2.up, ForceMode2D.Impulse);
        m_collider.enabled = false;
        yield return new WaitForSeconds(3);
        m_collider.enabled = true;
        GetComponent<Rigidbody2D>().velocity =Vector2.zero;
        m_animation.SetAnimation(0, "Charge", false).TimeScale = 2;
        yield return new WaitForAnimationComplete(m_animation.animationState, TombSoulAnimation.ANIMATION_CHARGE);
        m_animation.DoChargeRed();
        //yield return new WaitUntil(() => m_hasLaunched);
        yield return new WaitForSeconds(m_delay);
        m_willChase = true;
        //m_movement.MoveTo(m_target.position, m_soulSpeed);
        yield return null;
    }

    private void Update()
    {
        if (m_willChase)
        {
            m_movement.MoveTo(m_target.position, m_soulSpeed);
        }
    }
}
