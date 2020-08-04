using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using PlayerNew;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public class ComplexCharacterInfo
    {
        [SerializeField]
        private Character m_character;
        [SerializeField]
        private Attacker m_attacker;
        [SerializeField]
        private Damageable m_damageable;
        [SerializeField]
        private State.CharacterState m_state;
        [SerializeField]
        private Magic m_magic;
        [SerializeField]
        private CharacterPhysics2D m_physics;
        [SerializeField]
        private PlayerSensorList m_sensorList;
        [SerializeField]
        private Animator m_animator;
        [SerializeField]
        private AnimationParametersData m_animationParametersData;
        [SerializeField]
        private GroundednessHandle m_groundednessHandle;
        [SerializeField]
        private SkillResetRequester m_skillResetRequester;
        [SerializeField]
        private StatusEffectReciever m_statusEffectReciever;
        [SerializeField]
        private SurfaceDetector m_surfaceDector;
        [SerializeField]
        private PlayerBehaviour m_playerBehaviour;

        public ComplexCharacterInfo(GameObject character, AnimationParametersData animationParametersData)
        {
            m_character = character.GetComponentInChildren<Character>();
            m_attacker = character.GetComponentInChildren<Attacker>();
            m_damageable = character.GetComponentInChildren<Damageable>();
            m_state = character.GetComponentInChildren<State.CharacterState>();
            m_magic = character.GetComponentInChildren<Magic>();
            m_physics = character.GetComponentInChildren<CharacterPhysics2D>();
            m_sensorList = character.GetComponentInChildren<PlayerSensorList>();
            m_animator = character.GetComponentInChildren<Animator>();
            m_animationParametersData = animationParametersData;
            m_groundednessHandle = character.GetComponentInChildren<GroundednessHandle>();
            m_skillResetRequester = character.GetComponentInChildren<SkillResetRequester>();
            m_statusEffectReciever = character.GetComponentInChildren<StatusEffectReciever>();
            m_surfaceDector = character.GetComponentInChildren<SurfaceDetector>();
        }

        public Character character => m_character;
        public State.CharacterState state => m_state;
        public Animator animator => m_animator;
        public RaySensor GetSensor(PlayerSensorList.SensorType sensorType) => m_sensorList.GetSensor(sensorType);
        public AnimationParametersData animationParametersData => m_animationParametersData;
        public CharacterPhysics2D physics => m_physics;
        public GroundednessHandle groundednessHandle => m_groundednessHandle;
        public SkillResetRequester skillResetRequester => m_skillResetRequester;

        public Magic magic => m_magic;
        public Attacker attacker => m_attacker;
        public Damageable damageable => m_damageable;
        public StatusEffectReciever statusEffectReciever => m_statusEffectReciever;
        public SurfaceDetector surfaceDector => m_surfaceDector;

    }
}