using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay;

public class TombSoul : MonoBehaviour
{
    [SerializeField]
    private float m_soulSpeed;
    [SerializeField]
    private float m_riseSpeed;

    private ShiftingObjectPhysics2D m_physics;
    private Vector2 m_target;

    private void Awake()
    {
        m_physics = GetComponent<ShiftingObjectPhysics2D>();
    }

    private void Start()
    {
        StartCoroutine(SoulRoutine());
    }

    public void GetTarget(Vector2 target)
    {
        m_target = target;
    }

    private IEnumerator SoulRoutine()
    {
        //GetComponent<Rigidbody2D>().AddForce((m_riseSpeed + (Vector2.Distance(m_target, transform.position) * 0.35f)) * Vector2.up, ForceMode2D.Impulse);
        yield return new WaitForSeconds(3);
        GetComponent<Rigidbody2D>().AddForce((m_soulSpeed + (Vector2.Distance(m_target, transform.position) * 0.35f)) * transform.right, ForceMode2D.Impulse);
        Debug.Log("Soul Target: " + m_target);
        yield return null;
    }
}
