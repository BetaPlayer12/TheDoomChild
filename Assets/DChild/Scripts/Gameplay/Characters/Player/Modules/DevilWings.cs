using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class DevilWings : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        [SerializeField, MinValue(0)]
        private int m_sourceRequiredAmount;
        [SerializeField, MinValue(0)]
        private float m_sourceConsumptionRate;

        [SerializeField]
        private ParticleSystem m_wingsFX;

        private ILevitateState m_state;
        private Rigidbody2D m_rigidbody;
        private ICappedStat m_source;
        private float m_cacheGravity;
        private Animator m_animator;
        private int m_animationParameter;
        private float m_stackedConsumptionRate;

        public event EventAction<EventActionArgs> ExecuteModule;
        public event EventAction<EventActionArgs> End;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_state = info.state;
            m_rigidbody = info.rigidbody;
            m_cacheGravity = m_rigidbody.gravityScale;
            m_source = info.magic;
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsLevitating);
            m_stackedConsumptionRate = 0;
        }

        public void Cancel()
        {
            m_wingsFX.Stop();
            m_wingsFX.Clear();
            m_state.isLevitating = false;
            m_rigidbody.gravityScale = m_cacheGravity;
            m_rigidbody.velocity = Vector2.zero;
            m_animator.SetBool(m_animationParameter, false);
            m_stackedConsumptionRate = 0;

            End?.Invoke(this, EventActionArgs.Empty);
        }

        public void Execute()
        {
            m_source.ReduceCurrentValue(m_sourceRequiredAmount);
            m_wingsFX.Play();
            m_state.isLevitating = true;
            m_cacheGravity = m_rigidbody.gravityScale;
            m_rigidbody.gravityScale = 0;
            m_rigidbody.velocity = Vector2.zero;
            m_animator.SetBool(m_animationParameter, true);

            ExecuteModule?.Invoke(this, EventActionArgs.Empty);
        }

        public void MaintainHeight()
        {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, 0);
        }

        public bool HaveEnoughSourceForExecution() => m_sourceRequiredAmount <= m_source.currentValue;

        public bool HaveEnoughSourceForMaintainingHeight() => m_source.currentValue > 0;

        public void ConsumeSource()
        {
            m_stackedConsumptionRate += m_sourceConsumptionRate * GameplaySystem.time.deltaTime;
            Debug.Log(m_stackedConsumptionRate);
            if (m_stackedConsumptionRate >= 1)
            {
                var integer = Mathf.FloorToInt(m_stackedConsumptionRate);
                m_stackedConsumptionRate -= integer;
                m_source.ReduceCurrentValue(integer);
            }
        }
    }
}
