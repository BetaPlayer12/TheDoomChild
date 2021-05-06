using DChild.Gameplay.Characters.Players;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class SequenceIllusionPlatformHandle : MonoBehaviour
    {
        [SerializeField]
        private IllusionPlatform[] m_sequence;

        private PlayerCollisionSensor[] m_collisionSensorList;
        private PlayerCollisionSensor m_subscribeCollisionSensor;
        private int m_currentAppearingPlatformIndex;

        public void Reset()
        {
            m_currentAppearingPlatformIndex = 0;
            UseIndex(m_currentAppearingPlatformIndex);
        }

        public void RevealNext()
        {
            m_currentAppearingPlatformIndex++;
            UseIndex(m_currentAppearingPlatformIndex);
        }

        public void UseIndex(int index)
        {
            if (index == 0)
            {
                m_sequence[index].Appear(false);
                for (int i = 1; i < m_sequence.Length; i++)
                {
                    m_sequence[i]?.Disappear(false);
                }
            }
            else if (index < m_sequence.Length)
            {
                var indexToDisapear = index - 2;
                if (indexToDisapear >= 0)
                {
                    m_sequence[indexToDisapear]?.Disappear(false);
                }
                m_sequence[index]?.Appear(false);
            }

            SwitchSubscriptionTo(m_collisionSensorList[index]);
        }

        private void SwitchSubscriptionTo(PlayerCollisionSensor toSubscribeTo)
        {
            if (m_subscribeCollisionSensor)
            {
                m_subscribeCollisionSensor.CollisionDetected -= OnCollisionDetected;
            }

            m_subscribeCollisionSensor = toSubscribeTo;

            m_subscribeCollisionSensor.CollisionDetected += OnCollisionDetected;
        }

        private void OnCollisionDetected(object sender, EventActionArgs eventArgs)
        {
            RevealNext();
        }

        private void Awake()
        {
            m_collisionSensorList = new PlayerCollisionSensor[m_sequence.Length];
            for (int i = 0; i < m_sequence.Length; i++)
            {
                m_collisionSensorList[i] = m_sequence[i]?.GetComponentInChildren<PlayerCollisionSensor>() ?? null;
            }
            Reset();
        }
    }
}
