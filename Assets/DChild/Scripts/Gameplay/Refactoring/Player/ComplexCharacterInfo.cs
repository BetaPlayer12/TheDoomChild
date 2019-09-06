using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public class ComplexCharacterInfo
    {
        [SerializeField]
        private Character m_character;
        [SerializeField]
        private CharacterState m_state;
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

        public Character character => m_character;
        public CharacterState state => m_state;
        public Animator animator => m_animator;
        public RaySensor GetSensor(PlayerSensorList.SensorType sensorType) => m_sensorList.GetSensor(sensorType);
        public AnimationParametersData animationParametersData => m_animationParametersData;
        public CharacterPhysics2D physics => m_physics;
        public GroundednessHandle groundednessHandle => m_groundednessHandle;
        public SkillResetRequester skillResetRequester => m_skillResetRequester;

    }
}