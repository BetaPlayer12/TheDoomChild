using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Environment.Interractables;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageShake : MonoBehaviour
{
    [SerializeField]
    private float m_radiusOffset = 1;
    [SerializeField]
    private float m_duration = 1f;

    private bool m_shake=false;
    private Vector2 m_startingPos;
    private IHitToInteract m_interractable;

    private void Awake()
    {
        m_interractable = GetComponent<IHitToInteract>();
        m_interractable.OnHit += OnHit;
        m_startingPos.x = transform.position.x;
        m_startingPos.y = transform.position.y;
    }
    private void Update()
    {
        var offset = Random.insideUnitCircle;
        if (m_shake == true)
        {
            transform.position = m_startingPos + offset * m_radiusOffset;
        }
        
    }

    private void OnHit(object sender, HitDirectionEventArgs eventArgs)
    {
        StopAllCoroutines();
        m_shake = true;
        StartCoroutine(Shake());
        
    }
    IEnumerator Shake()
    {
       
        yield return new WaitForSeconds(m_duration);
        transform.position = m_startingPos;
        m_shake = false;

    }

}
