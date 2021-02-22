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

    private Vector2 m_startingPos;
    private IHitToInteract m_interractable;

    private void Awake()
    {
        m_interractable = GetComponent<IHitToInteract>();
        m_interractable.OnHit += OnHit;
        m_startingPos.x = transform.position.x;
        m_startingPos.y = transform.position.y;
    }

    private void OnHit(object sender, HitDirectionEventArgs eventArgs)
    {
        StopAllCoroutines();
        StartCoroutine(Shake(eventArgs.direction));
    }
    IEnumerator Shake(HorizontalDirection direction)
    {
        var offset = Random.insideUnitCircle;
        offset.x = 1 * (int)direction;
        transform.position = m_startingPos + offset * m_radiusOffset;
        yield return new WaitForSeconds(m_duration);
        transform.position = m_startingPos;
    }
}
