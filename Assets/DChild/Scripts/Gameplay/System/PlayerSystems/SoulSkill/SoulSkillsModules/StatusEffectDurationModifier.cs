using DChild.Gameplay.Combat.StatusAilment;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public class StatusEffectDurationModifier : ISoulSkillModule
    {
        [SerializeField, Range(-100, 100)]
        private int m_percentToAdd;

        private IPlayer m_reference;

        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            m_reference = player;
            player.statusEffectReciever.StatusRecieved += OnStatusRecieved;
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            player.statusEffectReciever.StatusRecieved -= OnStatusRecieved;
        }

        private void OnStatusRecieved(object sender, StatusEffectRecieverEventArgs eventArgs)
        {
            var duration = m_reference.statusEffectReciever.GetCurrentDurationOf(eventArgs.type);
            if(duration > 0)
            {
                duration += duration * (m_percentToAdd / 100f);
                m_reference.statusEffectReciever.SetCurrentDurationOf(eventArgs.type, duration);
            }
        }

    }
}