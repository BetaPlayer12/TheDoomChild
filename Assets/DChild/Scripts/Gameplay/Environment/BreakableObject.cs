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
        private void OnDestroyObject(object sender, EventActionArgs eventArgs)
        {
            m_onDestroy?.Invoke();
        }

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
    }
}
