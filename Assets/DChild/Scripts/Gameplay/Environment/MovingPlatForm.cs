using System.Collections;
using DChild.Gameplay.Characters.AI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatForm : MonoBehaviour
{
    

    [SerializeField]
    private float m_speed;
    [SerializeField]
    private Vector2[] m_waypoints;
    [ShowInInspector, OnValueChanged("ChangeDestination")]
    private int m_wayPointDestination;
    [ShowInInspector, ReadOnly]
    private int m_currentWayPoint;
    private int m_incrementerValue;

    private Rigidbody2D m_rigidbody;
    private Vector2 m_cacheDestination;
    private Vector2 m_cacheCurrentWaypoint;
    private int ListSize;

    private void ChangeDestination()
    {
        if (m_currentWayPoint != m_wayPointDestination)
        {

            int proposedIncrementerValue = 0;
            if (m_currentWayPoint > m_wayPointDestination)
            {
                proposedIncrementerValue = -1;
            }
            else
            {
                proposedIncrementerValue = 1;
            }

            if (proposedIncrementerValue != m_incrementerValue)
            {
                m_incrementerValue = proposedIncrementerValue;
                m_currentWayPoint += m_incrementerValue;
                m_cacheCurrentWaypoint = m_waypoints[m_currentWayPoint];


            }

            m_cacheDestination = m_waypoints[m_wayPointDestination]; //
        }

        enabled = true;
    }

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_currentWayPoint = m_wayPointDestination;
        m_cacheCurrentWaypoint = m_waypoints[m_currentWayPoint];
        ListSize = m_waypoints.Length;
        ChangeDestination();
    }

    private void Update()
    {
        if (m_rigidbody.position != m_cacheDestination)
        {
            m_rigidbody.position = Vector2.MoveTowards(m_rigidbody.position, m_cacheCurrentWaypoint, m_speed);
            if (m_rigidbody.position == m_cacheCurrentWaypoint && m_currentWayPoint != m_wayPointDestination)
            {
                m_currentWayPoint += m_incrementerValue;
                m_cacheCurrentWaypoint = m_waypoints[m_currentWayPoint];
            }
        }
        else
        {
            enabled = false;
        }
    }

    public void GoToNextWayPoint()
    {
        if (m_currentWayPoint < ListSize - 1)
        {
            m_wayPointDestination++;
            ChangeDestination();
        }
    }

    public void GoToPreviousWaypoint()
    {
        if (m_currentWayPoint > 0)
        {
            m_wayPointDestination--;
            ChangeDestination();
        }
    }

    public void GoDestination(int destination)
    {
        m_wayPointDestination = destination;

        ChangeDestination();
    }
}




