using DChild.Gameplay.Environment;
using Holysoft.Event;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay
{
    public class BlackBloodBloodPlatform : MonoBehaviour
    {
        [SerializeField]
        private BlackBloodBlob m_model;
        [SerializeField]
        private MovingPlatform m_movement;

        private Vector3 m_nextRotation;

        private void OnDestinationReached(object sender, MovingPlatform.UpdateEventArgs eventArgs)
        {
            StopAllCoroutines();

            if (eventArgs.currentWaypointIndex == (eventArgs.waypointCount - 1))
            {
                m_movement.GoDestination(0, false);
            }
            else
            {
                m_movement.GoToNextWayPoint();
            }

            //Debug.Log($"Angle {Vector3.SignedAngle(transform.position, m_movement.GetWayPoint(eventArgs.currentWaypointIndex), Vector3.right)}");
            var rotationEuler = m_model.transform.rotation.eulerAngles;
            rotationEuler.z -= 90f;
            m_nextRotation = rotationEuler;

            StartCoroutine(RotateRoutine());
        }

        private IEnumerator RotateRoutine()
        {
            m_movement.enabled = false;
            yield return m_model.GroundToRightRoutine();
            m_model.transform.rotation = Quaternion.Euler(m_nextRotation);
            m_model.DoMoveAnimation();
            m_movement.enabled = true;
        }

        private void OnBlobRessurected(object sender, EventActionArgs eventArgs)
        {
            m_model.transform.rotation = Quaternion.Euler(m_nextRotation);
            m_movement.enabled = true;
        }
        private void OnBlobDeath(object sender, EventActionArgs eventArgs)
        {
            m_movement.enabled = false;
        }

        private void Awake()
        {
            m_movement.DestinationReached += OnDestinationReached;
            m_movement.GoToNextWayPoint();
            m_model.DoMoveAnimation();
            m_movement.enabled = true;

            m_model.Ressurected += OnBlobRessurected;
            m_model.Death += OnBlobDeath;
        }

    }
}
