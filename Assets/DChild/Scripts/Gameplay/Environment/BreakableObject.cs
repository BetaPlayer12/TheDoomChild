using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class BreakableObject : MonoBehaviour
    {
        [SerializeField]
        private Damageable m_object;
        [ShowInInspector, OnValueChanged("SetObjectState")]
        private bool m_isDestroyed;

        [SerializeField, TabGroup("On Destroy")]
        private UnityEvent m_onDestroy;
        [SerializeField, TabGroup("On Fix")]
        private UnityEvent m_onFix;

        private Vector2 m_forceDirection;
        private float m_force;

        public void SetObjectState(bool isDestroyed)
        {
            m_isDestroyed = isDestroyed;
            if (m_isDestroyed == true)
            {
                m_onDestroy?.Invoke();
            }
            else
            {
                m_onFix?.Invoke();
            }
        }

        public void InstantiateDebris(GameObject debris)
        {
            var instance = Instantiate(debris, m_object.position, Quaternion.identity);
            instance.GetComponent<Debris>().SetInitialForceReference(m_forceDirection, m_force);
        }

        public void RecordForceReceived(Vector2 forceDirection, float force)
        {
            m_forceDirection = forceDirection;
            m_force = force;
        }

        private void OnDestroyObject(object sender, EventActionArgs eventArgs)
        {
            m_onDestroy?.Invoke();
        }

        // Start is called before the first frame update
        private void Awake()
        {
            m_object.Destroyed += OnDestroyObject;
            if (m_isDestroyed == true)
            {
                m_onDestroy?.Invoke();
            }
            else
            {
                m_onFix?.Invoke();
            }
        }

    }
}
