using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.UI
{

    public class PlayerHealthShaker : MonoBehaviour
    {
        [SerializeField]
        private Damageable m_damaeable;
        [SerializeField]
        private ObjectShaker m_shaker;
        [SerializeField, MinValue(0.1f)]
        private float m_duration;

        private float m_currentTime;

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            DoShake();
        }

        [Button]
        private void DoShake()
        {
            m_currentTime = m_duration;
            m_shaker.enabled = true;
            enabled = true;
        }

        private void Awake()
        {
            m_damaeable.DamageTaken += OnDamageTaken;
            enabled = false;
        }

        private void LateUpdate()
        {
            m_currentTime -= Time.deltaTime;
            if (m_currentTime <= 0)
            {
                m_shaker.enabled = false;
                enabled = false;
            }
        }
    }
}