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

    private IsolatedObjectPhysics2D m_physics;
    private AITargetInfo m_target;
    private TombSoulAnimation m_animation;
    private PhysicsMovementHandler2D m_movement;
    private bool m_willChase;
    private float m_launchTime;

    private void Awake()
    {
        m_physics = GetComponent<IsolatedObjectPhysics2D>();
        m_animation = GetComponent<TombSoulAnimation>();
        m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedObjectPhysics2D>(), transform);
    }

    private void Start()
    {
        StartCoroutine(SoulRoutine());
    }

    public void GetTarget(AITargetInfo target)
    {
        m_target = target;
    }

    public void Launch(float count)
    {
        m_launchTime = count;
    }

    //private void ThrowSoul()
    //{
    //    //Shoot Spit
    //    m_animation.DoCharge();
    //    var target = m_target.position; //No Parabola
    //    target = new Vector2(target.x, target.y - 2);
    //    Vector2 soulPos = transform.position;
    //    Vector3 v_diff = (target - soulPos);
    //    float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
    //    //transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
    //    //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //    transform.localRotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);

    //    GetComponent<Rigidbody2D>().AddForce((m_soulSpeed + (Vector2.Distance(target, transform.position) * 0.35f)) * transform.right, ForceMode2D.Impulse);
    //}

    private IEnumerator SoulRoutine()
    {
        GetComponent<Rigidbody2D>().AddForce(Random.Range(m_riseSpeed.x, m_riseSpeed.y) * Vector2.up, ForceMode2D.Impulse);
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(3);
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        m_animation.SetAnimation(0, "Charge", false).TimeScale = 2;
        yield return new WaitForAnimationComplete(m_animation.animationState, TombSoulAnimation.ANIMATION_CHARGE);
        m_animation.DoChargeRed();
        //yield return new WaitUntil(() => m_hasLaunched);
        yield return new WaitForSeconds(m_launchTime);
        m_willChase = true;
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
