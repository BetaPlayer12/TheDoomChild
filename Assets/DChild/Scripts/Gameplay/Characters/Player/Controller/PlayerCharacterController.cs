using DChild.Gameplay.Characters.Players.State;
using DChild.Inputs;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
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

        [Title("Model Reference")]
        [SerializeField]
        private GameObject m_behaviourContainer;
        [SerializeField]
        private CharacterState m_characterState;

        private ControllerEventArgs m_callArgs;

        private GroundController m_ground;
        private AirController m_air;
        private CombatController m_combatController;
        private PlayerInput m_input;

        public event EventAction<EventActionArgs> ControllerDisabled;

        public T GetSubController<T>() where T : ISubController => GetComponentInChildren<T>();

        private void InitializeSubControllers()
        {
            m_ground = GetComponent<GroundController>();
            m_ground.Initialize(m_behaviourContainer, m_skillRequester);
            m_air = GetComponent<AirController>();
            m_air.Initialize(m_behaviourContainer,m_skillRequester);
            m_combatController = GetComponent<CombatController>();
        }

        private void Awake()
        {
            m_input = GetComponentInParent<PlayerInput>();
            InitializeSubControllers();
        }

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
        }


        private void Update()
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
