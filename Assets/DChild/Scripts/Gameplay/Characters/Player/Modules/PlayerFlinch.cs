using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{

    public class PlayerFlinch : MonoBehaviour, IPlayerExternalModule
    {
        [SerializeField]
        private CountdownTimer m_flinchDuration = new CountdownTimer(1);
        private PlayerAnimation m_animation;

        private IBehaviourState m_behaviourState;
        private IFlinchState m_state;
        private IIsolatedTime m_time;
        private IFacing m_facing;

        public void Initialize(IPlayerModules player)
        {
            player.OnFlinch += OnFlinch;
            m_behaviourState = player.characterState;
            m_state = player.characterState;
            m_time = player.isolatedObject;
            m_animation = player.animation;
            m_facing = player;
        }

        private void Awake()
        {
            m_flinchDuration.CountdownEnd += OnFlinchDurationEnd;
        }

        private void OnFlinch(object sender, FlinchEventArgs eventArgs)
        {
            if (eventArgs.damageSource == RelativeDirection.Front)
            {
                m_animation.DoFlinchBack(m_facing.currentFacingDirection);
            }
            else
            {
                m_animation.DoFlinchForward(m_facing.currentFacingDirection);
            }

            m_behaviourState.waitForBehaviour = true;
            m_state.isFlinching = true;
            m_flinchDuration.Reset();
            enabled = true;
        }

        private void OnFlinchDurationEnd(object sender, EventActionArgs eventArgs)
        {
            m_behaviourState.waitForBehaviour = false;
            m_state.isFlinching = false;
            enabled = false;
           
        }

        private void Update() => m_flinchDuration.Tick(m_time.deltaTime);

    }

}