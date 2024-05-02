using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;
using static DChild.Gameplay.Characters.Enemies.FrankyEnvironmentReaction;

namespace DChild.Gameplay.Characters.Enemies
{
    public class FrankyEnvironmentReaction : MonoBehaviour
    {
        public enum EvironmentReactionType
        {
            Center,
            Left,
            Right,
            WaitForInput
        }

        [SerializeField]
        private FrankyAI m_franky;
        [SerializeField]
        private Animator m_frankyAnimator;

        [SerializeField]
        private EvironmentReactionType m_environmentReaction;

        private string m_reactionType;

        public UnityEvent m_evironmentEvent;

        private void PhaseDischargeReaction(object sender, EventActionArgs eventArgs)
        {
            m_environmentReaction = EvironmentReactionType.Center;
            FrankyEnvironmentReactionHandle(m_environmentReaction);
        }

        private void PushLeft(object sender, EventActionArgs eventArgs)
        {
            m_environmentReaction = EvironmentReactionType.Left;
            FrankyEnvironmentReactionHandle(m_environmentReaction);
        }

        private void PushRight(object sender, EventActionArgs eventArgs)
        {
            m_environmentReaction = EvironmentReactionType.Right;
            FrankyEnvironmentReactionHandle(m_environmentReaction);
        }

        [Button]
        public void FrankyEnvironmentReactionHandle(EvironmentReactionType evironmentReactionType)
        {

            if(evironmentReactionType == EvironmentReactionType.Center)
            {
                m_reactionType = "Center";
            }
            if (evironmentReactionType == EvironmentReactionType.Left)
            {
                m_reactionType = "Left";
            }
            if (evironmentReactionType == EvironmentReactionType.Right)
            {
                m_reactionType = "Right";
            }

            m_frankyAnimator.SetTrigger(m_reactionType);
            m_evironmentEvent?.Invoke();
            //m_frankyAnimator.SetBool(m_reactionType, true);
        }

        private void Start()
        {
            m_franky.PhaseDischargeAction += PhaseDischargeReaction;
            m_franky.ElectricPushLeft += PushLeft;
            m_franky.ElectricPushRight += PushRight;
        }

    }

}