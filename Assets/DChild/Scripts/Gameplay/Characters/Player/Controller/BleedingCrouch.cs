using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat.StatusAilment;
using DChild.Inputs;
using Holysoft.Collections;
using Holysoft.Event;
using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class BleedingCrouch : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private CountdownTimer m_holdTimer;

        //private IPlayerState m_state;
        private StatusEffectReciever m_statusEffectReciever;
        private bool m_isEffectStopped;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_statusEffectReciever = info.statusEffectReciever;
            m_statusEffectReciever.StatusEnd += OnStatusEnd;
            //m_state = info.state;
        }

        private void OnStatusEnd(object sender, StatusEffectRecieverEventArgs eventArgs)
        {
            m_holdTimer.Reset();
        }

        public void Handle(DirectionalInput input)
        {
            //var noOtherInputHeld = m_state.isMoving == false && m_state.waitForBehaviour == false;
            //if (m_state.isCrouched && input.isDownHeld && noOtherInputHeld)
            //{
            //    if(m_isEffectStopped == false)
            //    {
            //        m_statusEffectReciever.SetActive(StatusEffectType.Bleeding, false);
            //        m_isEffectStopped = true;
            //    }
            //    m_holdTimer.Tick(Time.deltaTime);
            //}
            //else
            //{
            //    if (m_isEffectStopped == true)
            //    {
            //        m_statusEffectReciever.SetActive(StatusEffectType.Bleeding, true);
            //        m_isEffectStopped = false;
            //    }
            //    m_holdTimer.Reset();
            //}
        }

        private void Awake()
        {
            m_holdTimer.CountdownEnd += OnTimerEnd;
        }

        private void OnTimerEnd(object sender, EventActionArgs eventArgs)
        {
            m_statusEffectReciever.StopStatusEffect(StatusEffectType.Bleeding);
        }
    }
}