using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class CombatReadiness : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        [SerializeField, HideLabel]
        private CombatReadinessStatsInfo m_configuration;

        private float m_timer;
        private ICombatReadinessState m_state;
        private Animator m_animator;
        private int m_animationParameter;

        public void Cancel()
        {
            m_timer = 0;
            m_state.isCombatReady = false;
            m_animator.SetBool(m_animationParameter, false);
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_timer = -1;
            m_state = info.state;
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.CombatMode);
        }

        public void SetConfiguration(CombatReadinessStatsInfo info)
        {
            m_configuration.CopyInfo(info);
        }

        public void HandleDuration()
        {
            if(m_timer > 0)
            {
                m_timer -= GameplaySystem.time.deltaTime;
                if(m_timer <= 0)
                {
                    m_state.isCombatReady = false;
                    m_animator.SetBool(m_animationParameter, false);
                }
            }
        }

        public void Execution()
        {
            m_timer = m_configuration.duration;
            m_state.isCombatReady = true;
            m_animator.SetBool(m_animationParameter, true);
        }
    }
}
