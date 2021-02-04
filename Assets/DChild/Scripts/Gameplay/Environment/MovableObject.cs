using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class MovableObject : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private SerializedVector3 m_position;
            [SerializeField]
            private bool m_canBeMoved;

            public SaveData(Vector3 position, bool canBeMoved)
            {
                m_position = position;
                m_canBeMoved = canBeMoved;
            }

            public Vector3 position => m_position;
            public bool canBeMoved => m_canBeMoved;

            ISaveData ISaveData.ProduceCopy() => new SaveData(position, canBeMoved);
        }

        [SerializeField]
        private bool m_isHeavy;
        [SerializeField]
        private GameObject m_parentObject;
        [SerializeField]
        private bool m_canBeMoved;
        [SerializeField, TabGroup("Grabbed")]
        private UnityEvent m_onGrabbed;
        [SerializeField, TabGroup("Let Go")]
        private UnityEvent m_onLetGo;

        public event EventAction<EventActionArgs> BecameUnmovable;

        public bool isHeavy => m_isHeavy;
        public bool canBeMove => m_canBeMoved;

        public void Load(ISaveData data)
        {
            var saveData = ((SaveData)data);
            transform.position = saveData.position;
            m_canBeMoved = saveData.canBeMoved;
        }

        public ISaveData Save() => new SaveData(transform.position, m_canBeMoved);

        public void SetMovable(bool value)
        {
            m_canBeMoved = value;
            if (value == false)
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;
                BecameUnmovable?.Invoke(this, EventActionArgs.Empty);
            }
        }

        public void SetGrabState(bool isGrabbed)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            if (isGrabbed)
            {
                m_onGrabbed?.Invoke();
            }
            else
            {
                rb.velocity = Vector2.zero;
                m_onLetGo?.Invoke();
            }
        }

        public GameObject GetParentObject()
        {
            return m_parentObject;
        }

        public void MoveObject(float direction, float moveForce)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            rb.velocity = new Vector2(direction * moveForce, 0);
        }
    }
}