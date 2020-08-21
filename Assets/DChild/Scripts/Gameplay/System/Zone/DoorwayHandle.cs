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
        private Transform m_prompt;
        [SerializeField]
        private bool m_forceExitFacing;
        [SerializeField, ShowIf("m_forceExitFacing")]
        private HorizontalDirection m_exitFacing;

        public float transitionDelay => 0;

        public bool needsButtonInteraction => true;

        public Vector3 promptPosition => m_prompt.position;

        public string prompMessage => "Enter";

        public void DoSceneTransition(Character character, TransitionType type)
        {
            if (type == TransitionType.Enter)
            {
                OnDoorwayEnter(character);
            }
            if (type == TransitionType.PostEnter)
            {
                OnDoorwayPostEnter(character);
            }
            else if (type == TransitionType.Exit && m_forceExitFacing)
            {
                character.SetFacing(m_exitFacing);
                OnDoorwayExit(character);
            }
            else if (type == TransitionType.Exit)
            {
                OnDoorwayExit(character);
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
            collisionState.forcedCurrentGroundedness = true;
        }
    }
}
