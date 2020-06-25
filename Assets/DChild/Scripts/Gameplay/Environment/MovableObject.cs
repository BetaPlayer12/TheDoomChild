using DChild.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class MovableObject : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private SerializedVector3 m_position;

            public SaveData(Vector3 position)
            {
                m_position = position;
            }

            public Vector3 position => m_position;
        }

        [SerializeField]
        private bool m_isHeavy;

        public bool isHeavy => m_isHeavy;

        public void Load(ISaveData data) => transform.position = ((SaveData)data).position;

        public ISaveData Save() => new SaveData(transform.position);
    }
}