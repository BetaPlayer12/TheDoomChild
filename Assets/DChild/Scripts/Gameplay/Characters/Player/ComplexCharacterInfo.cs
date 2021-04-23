using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using PlayerNew;
using Spine.Unity.Examples;
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
        private Modules.CharacterState m_state;
        [SerializeField]
        private Magic m_magic;
        [SerializeField]
        private PlayerModifierHandle m_modifier;
        [SerializeField]
        private CharacterPhysics2D m_physics;
        [SerializeField]
        private Rigidbody2D m_rigidbody;
        [SerializeField]
        private PlayerSensorList m_sensorList;
        [SerializeField]
        private Animator m_animator;
        [SerializeField]
        private AnimationParametersData m_animationParametersData;
        [SerializeField]
        private SkillResetRequester m_skillResetRequester;
        [SerializeField]
        private StatusEffectReciever m_statusEffectReciever;
        [SerializeField]
        private SurfaceDetector m_surfaceDector;
        [SerializeField]
        private PlayerBehaviour m_playerBehaviour;
        [SerializeField]
        private SkeletonGhost m_skeletonGhost;

        public ComplexCharacterInfo(GameObject character, AnimationParametersData animationParametersData)
        {
            m_character = character.GetComponentInChildren<Character>();
            m_attacker = character.GetComponentInChildren<Attacker>();
            m_damageable = character.GetComponentInChildren<Damageable>();
            m_state = character.GetComponentInChildren<Modules.CharacterState>();
            m_magic = character.GetComponentInChildren<Magic>();
            m_physics = character.GetComponentInChildren<CharacterPhysics2D>();
            m_sensorList = character.GetComponentInChildren<PlayerSensorList>();
            m_animator = character.GetComponentInChildren<Animator>();
            m_animationParametersData = animationParametersData;
            m_skillResetRequester = character.GetComponentInChildren<SkillResetRequester>();
            m_statusEffectReciever = character.GetComponentInChildren<StatusEffectReciever>();
            m_surfaceDector = character.GetComponentInChildren<SurfaceDetector>();
            m_skeletonGhost = character.GetComponentInChildren<SkeletonGhost>();
        }

        public Character character => m_character;
        public Modules.CharacterState state => m_state;
        public Animator animator => m_animator;
        public RaySensor GetSensor(PlayerSensorList.SensorType sensorType) => m_sensorList.GetSensor(sensorType);
        public AnimationParametersData animationParametersData => m_animationParametersData;
        public IPlayerModifer modifier => m_modifier;
        public CharacterPhysics2D physics => m_physics;
        public Rigidbody2D rigidbody => m_rigidbody;
        public SkillResetRequester skillResetRequester => m_skillResetRequester;
        public Magic magic => m_magic;
        public Attacker attacker => m_attacker;
        public Damageable damageable => m_damageable;
        public StatusEffectReciever statusEffectReciever => m_statusEffectReciever;
        public SurfaceDetector surfaceDector => m_surfaceDector;
        public SkeletonGhost skeletonGhost => m_skeletonGhost;
    }
}