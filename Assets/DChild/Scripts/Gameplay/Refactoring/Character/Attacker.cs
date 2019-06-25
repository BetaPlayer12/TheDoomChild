using DChild.Gameplay;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Combat
{
    public class Attacker : MonoBehaviour, IAttacker, IDamageDealer
    {
        [SerializeField]
        private Transform m_centerMass;
        [SerializeField, OnValueChanged("ApplyData")]
        private AttackerData m_data;

#if UNITY_EDITOR
        [SerializeField]
#endif
        private AttackerInfo m_info;

        public event EventAction<CombatConclusionEventArgs> TargetDamaged;

        public void Damage(TargetInfo targetInfo, BodyDefense targetDefense)
        {
            if (m_info.ignoreInvulnerability || !targetDefense.isInvulnerable)
            {
                var position = transform.position;
                for (int i = 0; i < m_info.damage.Count; i++)
                {
                    AttackInfo info = new AttackInfo(position, 0, 1, m_info.damage[i]);
                    var result = GameplaySystem.combatManager.ResolveConflict(info, targetInfo);
                    TargetDamaged?.Invoke(this, new CombatConclusionEventArgs(info, targetInfo.target, result));
                }
            }
        }

        public void SetDamage(params AttackDamage[] damage)
        {
            m_info.damage.Clear();
            m_info.damage.AddRange(damage);
        }

        public void SetCrit(int critChance, int critModifier)
        {
            m_info.critChance = critChance;
            m_info.critDamageModifier = critModifier;
        }
        public void SetData(AttackerData data)
        {
            m_data = data;
            m_info.Copy(m_data.info);
        }

        private void Awake()
        {
            m_info = new AttackerInfo();
            if (m_data != null)
            {
                m_info.Copy(m_data.info);
            }
        }


#if UNITY_EDITOR
        private void ApplyData()
        {
            m_info.Copy(m_data.info);
        }
#endif
    }
}