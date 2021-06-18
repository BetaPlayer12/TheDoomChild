using DChild.Gameplay.Characters.Players.Modules;
using PlayerNew;
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

        [SerializeField, OnValueChanged("OnDirectionChange")]
        private TravelDirection m_entranceDirection;
        [SerializeField, ShowIf("m_customTravelDirections")]
        private TravelDirection m_exitDirection;

        public float transitionDelay => 0.2f;

        public bool needsButtonInteraction => false;

        public Vector3 promptPosition => Vector3.zero;

        public string prompMessage => null;

        public void DoSceneTransition(Character character, TransitionType type)
        {
            switch (type)
            {
                case TransitionType.Enter:
                    OnPassagewayEnter(character, m_entranceDirection);
                    break;
                case TransitionType.PostEnter:
                    OnPassageWayPostEnter(character);
                    break;
                case TransitionType.Exit:
                    OnPassagewayExit(character, m_exitDirection);
                    break;
                case TransitionType.PostExit:
                    OnPassagewayPostExit();
                    break;
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

            Rigidbody2D rigidBody = character.GetComponent<Rigidbody2D>();
            rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            CharacterState collisionState = character.GetComponentInChildren<CharacterState>();
            collisionState.forcedCurrentGroundedness = true;
        }

        private void OnPassagewayExit(Character character, TravelDirection exitDirection)
        {
            var controller = GameplaySystem.playerManager.OverrideCharacterControls();
            controller.moveDirectionInput = (int)exitDirection;

            Rigidbody2D rigidBody = character.GetComponent<Rigidbody2D>();
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            CharacterState collisionState = character.GetComponentInChildren<CharacterState>();
            collisionState.forcedCurrentGroundedness = false;
        }

        private void OnPassagewayPostExit()
        {
            GameplaySystem.playerManager.StopCharacterControlOverride();
        }

#if UNITY_EDITOR
        [SerializeField, PropertyOrder(-1)]
        private bool m_customTravelDirections;

        private void OnDirectionChange()
        {
            if (m_customTravelDirections == false)
            {
                if (m_entranceDirection == TravelDirection.Left)
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
