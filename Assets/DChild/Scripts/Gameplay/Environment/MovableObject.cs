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

            ISaveData ISaveData.ProduceCopy() => new SaveData(position);
        }

        [SerializeField]
        private bool m_isHeavy;

        private Rigidbody2D m_rigidBody;

        public bool isHeavy => m_isHeavy;

        public void Load(ISaveData data) => transform.position = ((SaveData)data).position;

        public ISaveData Save() => new SaveData(transform.position);

        public void MoveObject(float direction, float moveForce)
        {
            m_rigidBody.velocity = new Vector2(direction * moveForce, 0);
            //m_rigidBody.AddForce(new Vector2(direction * moveForce, 0), ForceMode2D.Impulse);
        }

        private void Awake()
        {
            m_rigidBody = GetComponent<Rigidbody2D>();
        }
    }
}