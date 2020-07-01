using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Puzzles
{

    public class HauntTeleport : SerializedMonoBehaviour
    {
        [SerializeField]
        private int m_deathCounter;
        [SerializeField]
        private Damageable m_entity;
        [SerializeField]
        private Dictionary<int, Transform> m_dictionary;

        private IResetableAIBrain m_entitybrain;

        private void OnEntityDestroyed(object sender, EventActionArgs eventArgs)
        {
            m_deathCounter++;

            if (m_dictionary.ContainsKey(m_deathCounter))
            {
                m_entity.gameObject.SetActive(true);
                m_entity.transform.position = m_dictionary[m_deathCounter].position;
                m_entitybrain.ResetAI();
            }
            else
            {
                m_entity.gameObject.SetActive(false);
            }
        }

        void Start()
        {
            m_entitybrain = m_entity.GetComponent<IResetableAIBrain>();
            m_deathCounter = 0;
            m_entity.Destroyed += OnEntityDestroyed;
        }

        private void OnDestroy()
        {
            m_entity.Destroyed -= OnEntityDestroyed;
        }
    }
}