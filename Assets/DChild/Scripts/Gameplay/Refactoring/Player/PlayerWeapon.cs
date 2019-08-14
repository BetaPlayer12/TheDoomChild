using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Players
{
    public class PlayerWeapon : MonoBehaviour
    {
        [HorizontalGroup("Split")]

        [SerializeField, OnValueChanged("UpdateDamage", true), BoxGroup("Split/Base"), HideLabel]
        private AttackDamage m_baseDamage;
        [ShowInInspector, HideInEditorMode, OnValueChanged("UpdateDamage", true),
        BoxGroup("Split/Added"), HideLabel]
        private AttackDamage m_addedDamage;
        [ShowInInspector, HideInEditorMode, ReadOnly, BoxGroup("Total"), HideLabel]
        private List<AttackDamage> m_combinedDamage;
        private bool m_isInitialized;

        public AttackDamage[] damage { get => m_combinedDamage.ToArray(); }
        public event EventAction<EventActionArgs> DamageChange;

        public void SetAddedDamage(AttackDamage damage)
        {
            m_addedDamage = damage;
            CalculateCombinedDamage();
            DamageChange?.Invoke(this, EventActionArgs.Empty);
        }

        public void SetBaseDamage(AttackDamage damage)
        {
            m_baseDamage = damage;
            CalculateCombinedDamage();
            DamageChange?.Invoke(this, EventActionArgs.Empty);
        }

        private void CalculateCombinedDamage()
        {
            m_combinedDamage.Clear();
            m_combinedDamage.Add(m_baseDamage);
            if (m_addedDamage.value > 0)
            {
                m_combinedDamage.Add(m_addedDamage);
            }
        }

        public void Initialize()
        {
            if (m_isInitialized == false)
            {
                m_combinedDamage = new List<AttackDamage>();
                CalculateCombinedDamage();
                m_isInitialized = true;
            }
        }

        private void Awake()
        {
            Initialize();
        }

#if UNITY_EDITOR
        private void UpdateDamage()
        {
            CalculateCombinedDamage();
            DamageChange?.Invoke(this, EventActionArgs.Empty);
        }
#endif
    }
}