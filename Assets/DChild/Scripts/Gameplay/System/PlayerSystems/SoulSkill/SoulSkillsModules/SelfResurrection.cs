using System.Collections.Generic;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public class SelfResurrection : HandledSoulSkillModule
    {
        private class Handle : BaseHandle
        {
            private IPlayer m_reference;
            private float m_magicTransfer;
            private float m_magicToHealthModifier;

            public Handle(IPlayer reference, int magicTransfer, int healthReturn)
            {
                m_reference = reference;
                m_magicTransfer = magicTransfer / 100f;
                m_magicToHealthModifier = healthReturn / 100f;
            }

            public override void Initialize()
            {
                m_reference.OnDeath += OnDeath;
            }

            public override void Dispose()
            {
                m_reference.OnDeath -= OnDeath;
            }

            private void OnDeath(object sender, EventActionArgs eventArgs)
            {
                var magic = m_reference.magic;
                var toTransfer = Mathf.CeilToInt(magic.currentValue * m_magicTransfer);
                magic.ReduceCurrentValue(toTransfer);
                GameplaySystem.combatManager.Heal(m_reference, Mathf.CeilToInt(toTransfer * m_magicToHealthModifier));
            }
        }

        [SerializeField, Range(1, 100), SuffixLabel("%", overlay: true)]
        private int m_magicTransfer;
        [SerializeField, MinValue(1), SuffixLabel("%", overlay: true)]
        private int m_magicToHealthModifier;

        protected override BaseHandle CreateHandle(IPlayer player) => new Handle(player, m_magicTransfer, m_magicToHealthModifier);
    }
}