using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [AddComponentMenu("DChild/Gameplay/Player/Stat To Model Injector")]
    public class StatToModelInjector : MonoBehaviour
    {
        [SerializeField]
        private PlayerStats m_stats;
        [SerializeField]
        private PlayerWeapon m_weapon;
        [SerializeField]
        private AttackResistance m_attackResistance;
        [SerializeField]
        private StatusEffectResistance m_statusResistance;

        [Title("Model Reference")]
        [SerializeField]
        private Health m_health;
        [SerializeField]
        private Magic m_magic;
        [SerializeField]
        private Attacker m_attacker;
        [SerializeField]
        private StatusInflictor m_statusInflictor;
        [SerializeField]
        private AttackResistance m_modelAttackResistance;
        [SerializeField]
        private StatusEffectResistance m_modelStatusResistance;

        private void OnStatsChange(object sender, StatValueEventArgs eventArgs)
        {
            switch (eventArgs.stat)
            {
                case PlayerStat.Health:
                    m_health.SetMaxValue(eventArgs.value);
                    break;
                case PlayerStat.Magic:
                    m_magic.SetMaxValue(eventArgs.value);
                    break;
                case PlayerStat.Attack:
                case PlayerStat.MagicAttack:
                    m_attacker.SetDamage(CalculateDamage());
                    break;
                case PlayerStat._COUNT:
                    m_health.SetMaxValue(m_stats.GetTotalStat(PlayerStat.Health));
                    m_magic.SetMaxValue(m_stats.GetTotalStat(PlayerStat.Magic));
                    m_attacker.SetDamage(CalculateDamage());
                    break;
            }
        }

        private void InitializeValues()
        {
            m_health.SetMaxValue(m_stats.GetTotalStat(PlayerStat.Health));
            m_health.ResetValueToMax();
            m_magic.SetMaxValue(m_stats.GetTotalStat(PlayerStat.Magic));
            m_magic.ResetValueToMax();
            m_attacker.SetDamage(CalculateDamage());
            m_statusInflictor.SetInflictionList(m_weapon.statusInflictions);
            UpdateAttackResistance();
            UpdateStatusResistance();
        }

        private void UpdateAttackResistance()
        {
            // THIS WILL NOT WORK SINCE THE DAMAGETYPE IS NOW A ENUMFLAG DO SOMETHING
            var size = (int)DamageType.All;
            for (int i = 0; i < size; i++)
            {
                var attackType = (DamageType)i;
                var resistance = m_attackResistance.GetResistance(attackType);
                m_modelAttackResistance.SetResistance(attackType, resistance);
            }
        }

        private void UpdateStatusResistance()
        {
            var size = (int)StatusEffectType._COUNT;
            for (int i = 0; i < size; i++)
            {
                var statusEffect = (StatusEffectType)i;
                var resistance = m_statusResistance.GetResistance(statusEffect);
                m_modelStatusResistance.SetResistance(statusEffect, resistance);
            }
        }

        private Damage CalculateDamage()
        {
            var damage = m_weapon.damage;
            if (Damage.IsMagicDamage(damage.type))
            {
                var magicAttack = m_stats.GetTotalStat(PlayerStat.MagicAttack);
                damage.value += magicAttack;
            }
            else
            {
                var attack = m_stats.GetTotalStat(PlayerStat.Attack);
                damage.value += attack;
            }
            return damage;
        }

        private void OnDamageChange(object sender, EventActionArgs eventArgs)
        {
            m_attacker.SetDamage(CalculateDamage());
        }

        private void OnStatusInflictionUpdate(object sender, EventActionArgs eventArgs)
        {
            m_statusInflictor.SetInflictionList(m_weapon.statusInflictions);
        }

        private void OnAttackResistanceChange(object sender, AttackResistance.ResistanceEventArgs eventArgs)
        {
            if (eventArgs.type == DamageType.All)
            {
                UpdateAttackResistance();
            }
            else
            {
                m_modelAttackResistance.SetResistance(eventArgs.type, eventArgs.resistanceValue);
            }
        }

        private void OnStatusResistanceChange(object sender, StatusEffectResistance.ResistanceEventArgs eventArgs)
        {
            if (eventArgs.type == StatusEffectType._COUNT)
            {
                UpdateStatusResistance();
            }
            else
            {
                m_modelStatusResistance.SetResistance(eventArgs.type, eventArgs.value);
            }
        }

        private void Start()
        {
            InitializeValues();
            m_stats.StatsChanged += OnStatsChange;
            m_weapon.DamageChange += OnDamageChange;
            m_weapon.StatusInflictionUpdate += OnStatusInflictionUpdate;
            m_attackResistance.ResistanceChange += OnAttackResistanceChange;
            m_statusResistance.ResistanceChange += OnStatusResistanceChange;
        }

#if UNITY_EDITOR
        public void Initialize(GameObject character)
        {
            m_health = character.GetComponentInChildren<Health>();
            m_magic = character.GetComponentInChildren<Magic>();
            m_attacker = character.GetComponentInChildren<Attacker>();
            m_statusInflictor = character.GetComponentInChildren<StatusInflictor>();
            m_modelAttackResistance = character.GetComponentInChildren<AttackResistance>();
            m_modelStatusResistance = character.GetComponentInChildren<StatusEffectResistance>();
        }
#endif
    }
}