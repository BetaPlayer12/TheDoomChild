using UnityEngine;
using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using System.Collections.Generic;

public class WhipGrapple : MonoBehaviour
{
    private enum SelectionType
    {
        useNearestPoint,
        useNearestFront,
        useSelective
    }

    [SerializeField]
    private Transform m_bodyTransform;
    [SerializeField]
    private LayerMask m_mask;
    [SerializeField]
    private Vector2 m_overlapSize;

    private CharacterPhysics2D m_physics;
    private Transform m_transform;
    private Vector2 m_sensorPosition;
    private List<Collider2D> m_detectedGrappables;

    private float m_nearestDistance;
    private int m_nearestPoint;

    private SelectionType m_detectionType;

    //public void Initialize(IPlayerModules player)
    //{
    //    m_physics = player.physics;
    //    m_transform = player.physics.transform;
    //}

    public void Grapple(bool isActivated)
    {
        if (isActivated)
        {
            FindTargets();

            switch (m_detectionType)
            {
                case SelectionType.useNearestPoint:
                    if (m_detectedGrappables != null)
                        Move(FindNearestTarget().transform.position);
                    else break;
                    break;
                case SelectionType.useNearestFront:
                    break;
                case SelectionType.useSelective:
                    break;
            }
        }
        else
        {
            m_physics.simulateGravity = true;
        }
    }

    public void FindTargets()
    {
        var hit = Physics2D.OverlapBoxAll(m_sensorPosition, m_overlapSize, 0f, m_mask.value);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i] != null)
            {
                if (hit[i].tag == "Droppable")
                {
                    m_detectedGrappables.Add(hit[i]);
                }
            }
        }
    }

    private void Move(Vector3 targetPosition)
    {
        if (m_transform.position != targetPosition)
        {
            m_physics.simulateGravity = false;
            m_transform.position = Vector2.Lerp(m_transform.position, targetPosition, Time.deltaTime * 0.1f);
        }
        else
        {
            m_physics.simulateGravity = true;
        }
    }

    private Collider2D FindNearestTarget()
    {
        m_nearestDistance = Vector2.Distance(m_transform.position, m_detectedGrappables[0].transform.position);
        m_nearestPoint = 0;

        for (int i = 0; i < m_detectedGrappables.Count; i++)
        {
            var distance = Vector2.Distance(m_transform.position, m_detectedGrappables[i].transform.position);

            if (distance < m_nearestDistance)
            {
                m_nearestDistance = distance;
                m_nearestPoint = i;
            }
        }
        return m_detectedGrappables[m_nearestPoint];
    }

    private void OnDrawGizmos()
    {
        m_sensorPosition = m_bodyTransform.position;
        m_sensorPosition.y = m_sensorPosition.y + 10;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(m_sensorPosition, m_overlapSize);
    }
}
