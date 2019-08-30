using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.Skill;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Skill
{
    public class ShadowMorph : ShadowSkill, IShadow
    {
        [SerializeField]
        private CountdownTimer m_magicConsumptionInterval;
        [SerializeField, Min(1)]
        private int m_sustainMagicRequirement;
        [ShowInInspector]
        private bool m_hasMorphed;

        private Color m_color = Color.black;
        private Color m_defaultColor = Color.white;

        private SpriteRenderer m_visual;
        private IAttackResistance m_resistance;
        private IIsolatedTime m_time;

        public bool hasMorphed => m_hasMorphed;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);
            m_time = info.character.isolatedObject;
           // m_resistance = player.attackResistance;
        }

        public void BecomeAShadow(bool needsMagicToSustain)
        {
            m_hasMorphed = true;
            m_magicConsumptionInterval.Reset();
            //m_resistance.SetResistance(AttackType.Physical, AttackResistanceType.Immune);
            //m_visual.color = m_color;
            enabled = needsMagicToSustain;
        }

        public void BecomeNormal()
        {
            m_hasMorphed = false;
            //m_resistance.SetResistance(AttackType.Physical, AttackResistanceType.None);
            //m_visual.color = Color.white;
            enabled = false;
        }

        private void OnCoolOffEnd(object sender, EventActionArgs eventArgs)
        {
            m_magic.ReduceCurrentValue(m_sustainMagicRequirement * m_sustainMagicRequirement);
            m_magicConsumptionInterval.Reset();
            if (m_magic.currentValue <= 0)
            {
                BecomeNormal();
            }
        }

        private void Start()
        {
            m_visual = GetComponentInChildren<SpriteRenderer>();
            m_magicConsumptionInterval.CountdownEnd += OnCoolOffEnd;
            enabled = false;
        }

        private void Update()
        {
            m_magicConsumptionInterval.Tick(m_time.deltaTime);
        }
    }

}