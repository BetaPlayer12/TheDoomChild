using System;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat.StatusInfliction;
using DChild.Inputs;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerController : MonoBehaviour, IController, IPlayerExternalModule,IMainController
    {
        [SerializeField]
        private bool m_inputEnabled = true;
        private bool m_useStatusEffectController;
        private IPlayerState m_characterState;
        private IStatusEffectState m_statusState;
        private IPlayerSkills m_skills;
        private ControllerEventArgs m_callArgs;

        private StatusEffectController m_statusController;
        private CombatController m_combatController;//
        private GroundController m_ground;
        private AirController m_air;
        private AnimationController m_animation;//
        private PlayerInput m_input;

        public event EventAction<EventActionArgs> ControllerDisabled;

        public void Enable()
        {
            m_input.Enable();
            m_inputEnabled = true;
        }

        public void Disable()
        {
            Debug.Log("checking");
            m_input.Disable();
            m_inputEnabled = false;
            ControllerDisabled?.Invoke(this, EventActionArgs.Empty);
        }

        public void Initialize(IPlayerModules player)
        {
            m_statusController = GetComponent<StatusEffectController>();
            m_animation = GetComponent<AnimationController>();
            m_input = GetComponentInParent<PlayerInput>();
            m_ground = GetComponent<GroundController>();
            m_air = GetComponent<AirController>();
            m_combatController = GetComponent<CombatController>();
            m_combatController.Initialize(player.physics, player, player.animationState, player.characterState, player.characterState, player.animation, player.characterState, player.isolatedObject);
            m_characterState = player.characterState;
            m_statusState = player.statusEffectState;
            m_skills = player.skills;
            player.statusEffectState.StateChange += OnStatusEffectChange;
        }

        private void OnStatusEffectChange(object sender, StatusEffectAfflictionEventArgs eventArgs)
        {
            //This does not consider multiple status effect that needs the use of statusEffectController
            switch (eventArgs.statusEffectType)
            {
                case StatusEffectType.Frozen:
                case StatusEffectType.Petrify:
                    m_useStatusEffectController = eventArgs.isAffected;
                    break;
            }

        }

        private void Start()
        {
            m_callArgs = new ControllerEventArgs(m_input);
        }

        private void FixedUpdate()
        {
            if (m_useStatusEffectController == false)
            {
                if (m_characterState.waitForBehaviour || m_inputEnabled == false)
                    return;

                if (m_characterState.isGrounded)
                {
                    m_ground.CallFixedUpdate(m_characterState, m_skills, m_callArgs);
                }
                else
                {
                    m_air.CallFixedUpdate(m_characterState, m_skills, m_callArgs);
                }
            }
        }

        private void Update()
        {
            if (m_useStatusEffectController)
            {
                m_statusController?.CallUpdate(m_statusState, m_callArgs);
            }
            else
            {
                m_combatController?.CallUpdate(m_characterState, m_callArgs);

                if (m_characterState.waitForBehaviour || m_inputEnabled == false)
                    return;

                if (m_characterState.isGrounded)
                {
                    m_ground.CallUpdate(m_characterState, m_skills, m_callArgs);
                }
                else
                {
                    m_air.CallUpdate(m_characterState, m_skills, m_callArgs);
                }
            }
        }

        private void LateUpdate()
        {
            if (m_characterState.waitForBehaviour)
                return;

            m_animation.CallLateUpdate(m_characterState);
        }

    }
}