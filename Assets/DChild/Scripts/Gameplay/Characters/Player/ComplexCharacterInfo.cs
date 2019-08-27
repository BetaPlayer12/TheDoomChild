using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public class ComplexCharacterInfo
    {
        [SerializeField]
        private Character m_character;
        [SerializeField]
        private Damageable m_damageable;
        [SerializeField]
        private CharacterState m_state;
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

        public Character character => m_character;
        public CharacterState state => m_state;
        public Animator animator => m_animator;
        public RaySensor GetSensor(PlayerSensorList.SensorType sensorType) => m_sensorList.GetSensor(sensorType);
        public AnimationParametersData animationParametersData => m_animationParametersData;
        public CharacterPhysics2D physics => m_physics;
        public GroundednessHandle groundednessHandle => m_groundednessHandle;
        public SkillResetRequester skillResetRequester => m_skillResetRequester;

        public Magic magic => m_magic;
        public Damageable damageable => m_damageable;
        public StatusEffectReciever statusEffectReciever => m_statusEffectReciever;
        public SurfaceDetector surfaceDector => m_surfaceDector;
    }
}