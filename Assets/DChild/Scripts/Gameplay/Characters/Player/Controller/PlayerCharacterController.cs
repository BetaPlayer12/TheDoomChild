using DChild.Gameplay.Characters.Players.State;
using DChild.Inputs;
using Refactor.DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerCharacterController : MonoBehaviour
    {
        [SerializeField]
        private PlayerSkills m_skills;

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

        private void InitializeSubControllers()
        {
            m_ground = GetComponent<GroundController>();
            m_air = GetComponent<AirController>();
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
