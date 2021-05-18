using DChild.Gameplay.Characters.Players.Behaviour;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class IdleHandle : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        [SerializeField, MinValue(1f)]
        private float m_playExtendedIdleAnimAfter = 1f;
        [SerializeField, MinValue(1)]
        private int m_maxIdleAnimCount;

        private Animator m_animator;
        private int m_idleAnimationParameter;
        private int m_idleStateAnimationParameter;
        private int m_currentIdleIndex;
        private float m_timer;
        private bool m_isInIdle;


        public void Initialize(ComplexCharacterInfo info)
        {
            m_animator = info.animator;
            m_idleAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsIdle);
            m_idleStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IdleState);
            m_currentIdleIndex = 0;
        }

        public void GenerateRandomState()
        {
            m_currentIdleIndex = Random.Range(1, m_maxIdleAnimCount+1);
            m_animator.SetInteger(m_idleStateAnimationParameter, m_currentIdleIndex);
        }

        public void Cancel()
        {
            m_isInIdle = false;
            m_animator.SetBool(m_idleAnimationParameter, false);
            m_currentIdleIndex = 0;
            m_animator.SetInteger(m_idleStateAnimationParameter, m_currentIdleIndex);
        }

        public void Execute(bool allowExtendedIdle)
        {
            m_animator.SetBool(m_idleAnimationParameter, true);
            if (allowExtendedIdle == true)
            {
                if (m_isInIdle && m_currentIdleIndex == 0)
                {
                    if (m_timer > 0)
                    {
                        m_timer -= GameplaySystem.time.deltaTime;
                        if (m_timer <= 0)
                        {
                            GenerateRandomState();
                        }
                    }
                }
                else
                {
                    m_isInIdle = true;
                    m_timer = m_playExtendedIdleAnimAfter;
                }
            }
            else
            {
                if (m_currentIdleIndex != 0)
                {
                    m_currentIdleIndex = 0;
                    m_animator.SetInteger(m_idleStateAnimationParameter, 0);
                }

                m_isInIdle = true;
            }
        }
    }
}
