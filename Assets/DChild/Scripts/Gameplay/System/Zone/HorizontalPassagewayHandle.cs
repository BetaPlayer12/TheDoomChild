using DChild.Gameplay;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Environment;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment
{

    [System.Serializable]
    public struct HorizontalPassagewayHandle : ISwitchHandle
    {
        private enum TravelDirection
        {
            Left = -1,
            Right = 1,
        }

        [SerializeField,OnValueChanged("OnDirectionChange")]
        private TravelDirection m_entranceDirection;
        [SerializeField,ShowIf("m_customTravelDirections")]
        private TravelDirection m_exitDirection;

        public float transitionDelay => 1;

        public void DoSceneTransition(Character character, TransitionType type)
        {
            if (type == TransitionType.Enter)
            {
                OnPassagewayEnter(character, m_entranceDirection);
            }
            else if (type == TransitionType.PostEnter)
            {
                OnPassageWayPostEnter(character);
            }
            else if (type == TransitionType.Exit)
            {
                OnPassagewayExit(character, m_exitDirection);
            }
        }

        private void OnPassagewayEnter(Character character, TravelDirection travelDirection)
        {
            var controller = GameplaySystem.playerManager.OverrideCharacterControls();
            controller.moveDirectionInput = (int)travelDirection;

        }

        private void OnPassageWayPostEnter(Character character)
        {
            var controller = GameplaySystem.playerManager.OverrideCharacterControls();

            controller.moveDirectionInput = 0;
            character.physics.SetVelocity(Vector2.zero);
            character.physics.simulateGravity = false;
            var groundednessHandle = character.GetComponentInChildren<GroundednessHandle>();
            groundednessHandle.enabled = false;
        }

        private void OnPassagewayExit(Character character, TravelDirection exitDirection)
        {
            var controller = GameplaySystem.playerManager.OverrideCharacterControls();
            controller.moveDirectionInput = (int)exitDirection;
            character.physics.simulateGravity = true;
            var groundednessHandle = character.GetComponentInChildren<GroundednessHandle>();
            groundednessHandle.enabled = true;
        }

#if UNITY_EDITOR
        [SerializeField, PropertyOrder(-1)]
        private bool m_customTravelDirections;

        private void OnDirectionChange()
        {
            if(m_customTravelDirections == false)
            {
                if(m_entranceDirection == TravelDirection.Left)
                {
                    m_exitDirection = TravelDirection.Right;
                }
                else
                {
                    m_exitDirection = TravelDirection.Left;
                }
            }
        }
#endif

    }
}
