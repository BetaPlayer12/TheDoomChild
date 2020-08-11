using DChild.Gameplay.Characters.Players.State;

using DChild.Gameplay.Combat.StatusAilment;
using DChild.Inputs;
using Holysoft.Event;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;
using DChild.Gameplay.Characters.Players.Behaviour;

namespace DChild.Gameplay.Characters.Players.Modules
{

    [AddComponentMenu("DChild/Gameplay/Player/Controller/Player Character Controller Old")]
    public class PlayerCharacterControllerOld : MonoBehaviour/*, IMainController*/
    {
//        [SerializeField]
//        private PlayerSkills m_skills;
//        [SerializeField]
//        private SkillResetRequester m_skillRequester;

//        [SerializeField]
//        private StatusEffectReciever m_statusReciever;
//        [ShowInInspector, HideInEditorMode]
//        private bool m_enabled;

//        [Title("Model Reference")]
//        [SerializeField]
//        private GameObject m_behaviourContainer;
//        [SerializeField]
//        private State.CharacterState m_characterState;
//        [SerializeField]
//        private GroundednessHandle m_groundedness;

//        private ControllerEventArgs m_callArgs;

//        private GroundController m_ground;
//        private AirController m_air;
//        private CombatController m_combatController;

//        private StatusController m_statusController;

//        private PlayerInputOld m_input;

//        public event EventAction<EventActionArgs> ControllerDisabled;

//        public void Enable()
//        {
//            m_enabled = true;
//        }

//        public void Disable()
//        {
//            m_enabled = false;
//            ControllerDisabled?.Invoke(this, EventActionArgs.Empty);
//        }

//        private void InitializeSubControllers()
//        {
//            m_ground = GetComponent<GroundController>();
//            m_ground.Initialize(m_behaviourContainer, m_skillRequester);
//            m_air = GetComponent<AirController>();
//            m_air.Initialize(m_behaviourContainer, m_skillRequester);
//            m_combatController = GetComponent<CombatController>();
//            m_combatController.Initialize(m_behaviourContainer);
//            m_statusController = GetComponent<StatusController>();
//            m_statusController.Initialize(m_behaviourContainer, m_statusReciever);

//        }

//        private void Awake()
//        {
//            m_input = GetComponentInParent<PlayerInputOld>();
//            m_enabled = true;
//            InitializeSubControllers();
//        }
//        private void Start()
//        {
//            m_callArgs = new ControllerEventArgs(m_input);
//            var controllableModules = m_behaviourContainer.GetComponentsInChildren<IControllableModule>();
//            for (int i = 0; i < controllableModules.Length; i++)
//            {
//                controllableModules[i].ConnectTo(this);
//            }
//            m_groundedness.Initialize();
//        }

//        private void FixedUpdate()
//        {
//            if (m_characterState.isGrounded)
//            {
//                m_groundedness.HandleGround();
//            }
//            else
//            {


//                m_groundedness.HandleMidAir();
//                if (m_characterState.isDashing == false)
//                {
//                    m_groundedness.HandleLand();
//                }


//            }

//            if (m_enabled)
//            {
//                if (m_characterState.waitForBehaviour)
//                    return;

//                if (m_characterState.isGrounded)
//                {

//                    m_ground.CallFixedUpdate(m_characterState, m_skills, m_callArgs);

//                }
//                else
//                {

//                    m_air.CallFixedUpdate(m_characterState, m_skills, m_callArgs);
//                }
//            }
//        }


//        private void Update()
//        {
//            if (m_statusController?.isActive ?? false)
//            {
//                m_statusController.CallUpdate(m_characterState, m_callArgs);
//            }

//            if (m_enabled)
//            {
//                if (m_characterState.waitForBehaviour)
//                    return;

//                m_combatController?.CallUpdate(m_characterState, m_callArgs);

//                if (m_characterState.waitForBehaviour)
//                    return;

//                if (m_characterState.isGrounded)
//                {
//                    m_ground.CallUpdate(m_characterState, m_skills, m_callArgs);
//                }
//                else
//                {
//                    m_air.CallUpdate(m_characterState, m_skills, m_callArgs);
//                }
//            }
//        }

//#if UNITY_EDITOR
//        public void Initialize(GameObject character, GameObject behaviour)
//        {
//            m_behaviourContainer = behaviour;
//            m_characterState = character.GetComponentInChildren<State.CharacterState>();
//            m_groundedness = character.GetComponentInChildren<GroundednessHandle>();
//        }
//#endif
    }
}
