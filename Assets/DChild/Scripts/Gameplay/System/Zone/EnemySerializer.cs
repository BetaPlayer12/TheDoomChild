using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems.Serialization
{
    [RequireComponent(typeof(ISerializableEnemy))]
    public class EnemySerializer : MonoBehaviour
    {
        private ISerializableEnemy m_serializableCharacter;
        [SerializeField]
        private bool m_isAlive = true;

        public void InitializeEnemyAs(bool isAlive)
        {
            m_isAlive = isAlive;
            m_serializableCharacter.InitializeAs(m_isAlive);
        }

        private void Awake()
        {
            m_serializableCharacter = GetComponent<ISerializableEnemy>();
        }

        private void Start()
        {
            m_serializableCharacter.InitializeAs(m_isAlive);
        }

#if UNITY_EDITOR
        public bool isAlive
        {
            get
            {
                return m_isAlive;
            }

            set
            {
                m_isAlive = value;
            }
        }
#endif
    }
}