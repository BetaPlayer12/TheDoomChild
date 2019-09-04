using DChild.Gameplay.Characters.Players.State;
<<<<<<< HEAD
using DChild.Inputs;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
=======
using DChild.Gameplay.Combat.StatusAilment;
using DChild.Inputs;
using Holysoft.Event;
using DChild.Gameplay.Characters.Players;
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerCharacterController : MonoBehaviour, IMainController
    {
        [SerializeField]
        private PlayerSkills m_skills;
        [SerializeField]
        private SkillResetRequester m_skillRequester;
<<<<<<< HEAD
=======
        [SerializeField]
        private StatusEffectReciever m_statusReciever;
        [ShowInInspector, HideInEditorMode]
        private bool m_enabled;
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818

        [Title("Model Reference")]
        [SerializeField]
        private GameObject m_behaviourContainer;
        [SerializeField]
        private CharacterState m_characterState;

        private ControllerEventArgs m_callArgs;

        private GroundController m_ground;
        private AirController m_air;
        private CombatController m_combatController;
<<<<<<< HEAD
=======
        private StatusController m_statusController;
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
        private PlayerInput m_input;

        public event EventAction<EventActionArgs> ControllerDisabled;

<<<<<<< HEAD
        public T GetSubController<T>() where T : ISubController => GetComponentInChildren<T>();
=======
        public void Enable()
        {
            m_enabled = true;
        }

        public void Disable()
        {
            m_enabled = false;
            ControllerDisabled?.Invoke(this, EventActionArgs.Empty);
        }
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818

        private void InitializeSubControllers()
        {
            m_ground = GetComponent<GroundController>();
            m_ground.Initialize(m_behaviourContainer, m_skillRequester);
            m_air = GetComponent<AirController>();
<<<<<<< HEAD
            m_air.Initialize(m_skillRequester);
            m_combatController = GetComponent<CombatController>();
=======
            m_air.Initialize(m_behaviourContainer, m_skillRequester);
            m_combatController = GetComponent<CombatController>();
            m_combatController.Initialize(m_behaviourContainer);
            m_statusController = GetComponent<StatusController>();
            m_statusController.Initialize(m_behaviourContainer, m_statusReciever);
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
        }

        private void Awake()
        {
            m_input = GetComponentInParent<PlayerInput>();
<<<<<<< HEAD
            InitializeSubControllers();
        }


=======
            m_enabled = true;
            InitializeSubControllers();
        }

>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
        private void Start()
        {
            m_callArgs = new ControllerEventArgs(m_input);
            var controllableModules = m_behaviourContainer.GetComponentsInChildren<IControllableModule>();
            for (int i = 0; i < controllableModules.Length; i++)
            {
                controllableModules[i].ConnectTo(this);
            }
        }

        private void FixedUpdate()
        {
<<<<<<< HEAD
            if (m_characterState.waitForBehaviour)
                return;

            if (m_characterState.isGrounded)
            {
                m_ground.CallFixedUpdate(m_characterState, m_skills, m_callArgs);
            }
            else
            {
                m_air.CallFixedUpdate(m_characterState, m_skills, m_callArgs);
=======
            if (m_enabled)
            {
                if (m_characterState.waitForBehaviour)
                    return;

                if (m_characterState.isGrounded)
                {
                    m_ground.CallFixedUpdate(m_characterState, m_skills, m_callArgs);
                }
                else
                {
                    m_air.CallFixedUpdate(m_characterState, m_skills, m_callArgs);
                }
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
            }
        }


        private void Update()
        {
<<<<<<< HEAD
            if (m_characterState.waitForBehaviour)
                return;

            m_combatController?.CallUpdate(m_characterState, m_callArgs);

            if (m_characterState.waitForBehaviour)
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

=======
            if (m_statusController?.isActive ?? false)
            {
                m_statusController.CallUpdate(m_characterState, m_callArgs);
            }

            if (m_enabled)
            {
                if (m_characterState.waitForBehaviour)
                    return;

                m_combatController?.CallUpdate(m_characterState, m_callArgs);

                if (m_characterState.waitForBehaviour)
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
    }
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
}
