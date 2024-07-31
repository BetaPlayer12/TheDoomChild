using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    [System.Serializable]
    public class PlayerPositionLock : IStatusEffectUpdatableModule
    {
        [SerializeField,MinValue(0)]
        private float m_maxXDistanceChange;
        private Vector2 m_lockIntoPosition;

        private Character m_character;
        private InputTranslator m_playerInput; //This is very dangerous need to find a way to check if player is moving without checking for input

        public PlayerPositionLock(float maxXDistanceChange)
        {
            m_maxXDistanceChange = maxXDistanceChange;
        }

        public void CalculateWithDuration(float duration)
        {

        }

        public IStatusEffectUpdatableModule CreateCopy()
        {
            return new PlayerPositionLock(m_maxXDistanceChange);
        }

        public void Initialize(Character character)
        {
            m_character = character;
            m_lockIntoPosition = character.transform.localPosition;
            if(character = GameplaySystem.playerManager.player.character)
            {

                m_playerInput = GameplaySystem.playerManager.player.GetComponentInChildren<InputTranslator>();
                
            }
        }

        public void Update(float delta)
        {
            if (m_playerInput.horizontalInput != 0)
            {
                var currentlocalPosition = m_character.transform.localPosition;
                if(Mathf.Abs(m_lockIntoPosition.x - currentlocalPosition.x) > m_maxXDistanceChange)
                {
                    var toPlayerPosition = (Vector2)m_character.transform.localPosition - m_lockIntoPosition;
                    var signedMovement = Mathf.Sign(toPlayerPosition.x);

                    var proposedClampPosition = m_lockIntoPosition;
                    proposedClampPosition.x += signedMovement * m_maxXDistanceChange;
                    Debug.Log(proposedClampPosition);
                    m_character.transform.localPosition = proposedClampPosition;
                }
            }
            else
            {
                m_character.transform.localPosition = m_lockIntoPosition;
            }
        }
    }
}