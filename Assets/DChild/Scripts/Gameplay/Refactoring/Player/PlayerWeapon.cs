using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Players
{

    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField, OnValueChanged("SendBaseEvent", true)]
        private DamageList m_baseDamage;
        [ShowInInspector, ReadOnly, HideInEditorMode]
        List<AttackDamage> m_damage;
        private bool m_isInitialized;

        public AttackDamage[] baseDamage { get => m_baseDamage.values; }
        public AttackDamage[] damage { get => m_damage.ToArray(); }

        public event EventAction<EventActionArgs> BaseDamageChange;

        public void AddDamage(AttackDamage damage)
        {
            m_baseDamage.AddValue(damage.type, damage.damage);
            BaseDamageChange?.Invoke(this, EventActionArgs.Empty);
        }

        public void SetTotalDamage(params AttackDamage[] damages)
        {
            m_damage.Clear();
            m_damage.AddRange(damages);
        }

        public void Initialize()
        {
            if (m_isInitialized == false)
            {
                m_damage = new List<AttackDamage>();
                m_damage.AddRange(m_baseDamage.values);
                m_isInitialized = true;
            }
        }

        private void Awake()
        {
            Initialize();
        }

#if UNITY_EDITOR
        private void SendBaseEvent()
        {
            BaseDamageChange?.Invoke(this, EventActionArgs.Empty);
        }
#endif
    }
}