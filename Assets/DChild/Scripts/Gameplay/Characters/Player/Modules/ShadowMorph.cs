using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ShadowMorph : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule, IResettableBehaviour
    {
        [SerializeField, MinValue(0)]
        private int m_sourceRequiredAmount;
        [SerializeField, MinValue(0)]
        private int m_sourceConsumptionRate;

        private ICappedStat m_source;
        private IShadowModeState m_state;
        private Animator m_animator;
        private int m_animationParameter;
        private float m_stackedConsumptionRate;

        public event EventAction<EventActionArgs> ExecuteShadowMorph;
        public event EventAction<EventActionArgs> EndShadowMorphExecution;

        public bool IsInShadowMode() => m_state.isInShadowMode;

        public bool HaveEnoughSourceForExecution() => m_sourceRequiredAmount <= m_source.currentValue;

        public void ConsumeSource()
        {
            m_stackedConsumptionRate += m_sourceConsumptionRate * GameplaySystem.time.deltaTime;

            if (m_stackedConsumptionRate >= 1)
            {
                var integer = Mathf.FloorToInt(m_stackedConsumptionRate);
                m_stackedConsumptionRate -= integer;
                m_source.ReduceCurrentValue(integer);
            }
        }

        public void EndExecution()
        {
            m_state.isInShadowMode = true;
            m_state.waitForBehaviour = false;

            EndShadowMorphExecution?.Invoke(this, EventActionArgs.Empty);
            Debug.Log("Dugang dugang");
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            m_animator.SetBool(m_animationParameter, true);

            ExecuteShadowMorph?.Invoke(this, EventActionArgs.Empty);
        }

        public void Cancel()
        {
            m_state.isInShadowMode = false;
            m_animator.SetBool(m_animationParameter, false);
            m_stackedConsumptionRate = 0;
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_source = info.magic;
            m_state = info.state;
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.ShadowMode);
            m_stackedConsumptionRate = 0;
        }

        public void Reset()
        {
            m_state.isInShadowMode = false;
        }
    }
}
