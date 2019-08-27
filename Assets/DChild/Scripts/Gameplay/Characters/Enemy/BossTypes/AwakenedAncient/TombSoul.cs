using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay;
using DChild.Gameplay.Characters.AI;

public class TombSoul : MonoBehaviour
{
    [SerializeField]
    private float m_soulSpeed;
    [SerializeField]
    private float m_riseSpeed;

    private IsolatedObjectPhysics2D m_physics;
    private AITargetInfo m_target;
    private TombSoulAnimation m_animation;
    private PhysicsMovementHandler2D m_movement;
    private bool m_willChase;

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
        GetComponent<Rigidbody2D>().AddForce(m_riseSpeed * Vector2.up, ForceMode2D.Impulse);
        yield return new WaitForSeconds(3);
        m_willChase = true;
        m_animation.DoCharge();
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
