using Holysoft.Event;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerDeath : MonoBehaviour, IComplexCharacterModule
    {
        private Damageable m_source;
        private Animator m_animator;
        private string m_deathParameter;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_source = info.damageable;
            m_source.Destroyed += OnDeath;
            m_animator = info.animator;
            m_deathParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Death);
        }

        private void OnDeath(object sender, EventActionArgs eventArgs)
        {
            m_source.SetHitboxActive(false);
            m_animator.SetTrigger(m_deathParameter);
        }
    }
}