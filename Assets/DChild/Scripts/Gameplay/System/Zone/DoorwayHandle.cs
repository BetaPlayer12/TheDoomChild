using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players.Modules;
using PlayerNew;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [System.Serializable]
    public class DoorwayHandle : ISwitchHandle
    {
        [SerializeField]
        private Transform m_promptSource;
        [SerializeField]
        private Vector3 m_promptOffset;
        [SerializeField]
        private bool m_forceExitFacing;
        [SerializeField, ShowIf("m_forceExitFacing")]
        private HorizontalDirection m_exitFacing;

        public float transitionDelay => 0;

        public bool needsButtonInteraction => true;

        public Vector3 promptPosition => m_promptSource.position + m_promptOffset;

        public string prompMessage => "Enter";

        public void DoSceneTransition(Character character, TransitionType type)
        {
            switch (type)
            {
                case TransitionType.Enter:
                    OnDoorwayEnter(character);
                    break;
                case TransitionType.PostEnter:
                    OnDoorwayPostEnter(character);
                    break;
                case TransitionType.Exit:
                    if (m_forceExitFacing)
                    {
                        character.SetFacing(m_exitFacing);
                    }

                    OnDoorwayExit(character);
                    break;
                case TransitionType.PostExit:
                    OnDoorwayPostExit();
                    break;
            }
        }

        private void OnDoorwayEnter(Character character)
        {
            var controller = GameplaySystem.playerManager.OverrideCharacterControls();
            controller.moveDirectionInput = 0;
        }

        private void OnDoorwayPostEnter(Character character)
        {
            var controller = GameplaySystem.playerManager.OverrideCharacterControls();
            controller.moveDirectionInput = 0;

            Rigidbody2D rigidBody = character.GetComponent<Rigidbody2D>();
            rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            CharacterState collisionState = character.GetComponentInChildren<CharacterState>();
            collisionState.forcedCurrentGroundedness = true;
        }

        private void OnDoorwayExit(Character character)
        {
            var controller = GameplaySystem.playerManager.OverrideCharacterControls();
            controller.moveDirectionInput = 0;

            Rigidbody2D rigidBody = character.GetComponent<Rigidbody2D>();
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            CharacterState collisionState = character.GetComponentInChildren<CharacterState>();
            collisionState.forcedCurrentGroundedness = false;
        }

        private void OnDoorwayPostExit()
        {
            GameplaySystem.playerManager.StopCharacterControlOverride();
        }
    }
}
