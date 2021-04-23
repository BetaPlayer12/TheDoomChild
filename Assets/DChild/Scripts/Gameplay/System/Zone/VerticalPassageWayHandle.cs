using DChild.Gameplay.Characters;
using PlayerNew;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [System.Serializable]
    public struct VerticalPassageWayHandle : ISwitchHandle
    {
        private enum TravelDirection
        {
            Up,
            Down
        }

        [SerializeField, OnValueChanged("OnDirectionChange")]
        private TravelDirection m_entranceDirection;
        [SerializeField, ShowIf("m_customTravelDirections")]
        private TravelDirection m_exitDirection;
        [SerializeField]
        private Vector2 m_upVelocity;
        [SerializeField]
        private bool m_forceExitFacing;
        [SerializeField, ShowIf("m_forceExitFacing")]
        private HorizontalDirection m_exitFacing;
        [SerializeField, MinValue(0)]
        private float m_transitionDelay;

        private static Coroutine forceFloatCoroutine;

        public float transitionDelay => m_transitionDelay;

        public bool needsButtonInteraction => false;

        public Vector3 promptPosition => Vector3.zero;

        public string prompMessage => null;

        public void DoSceneTransition(Character character, TransitionType type)
        {
            var controller = GameplaySystem.playerManager.OverrideCharacterControls();
            controller.moveDirectionInput = 0;
            var characterPhysics = character.GetComponent<Rigidbody2D>();

            switch (type)
            {
                case TransitionType.Enter:
                    if (m_entranceDirection == TravelDirection.Up)
                    {
                        character.StartCoroutine(UpEntranceRoutine(characterPhysics));
                    }
                    else
                    {
                        characterPhysics.constraints = RigidbodyConstraints2D.FreezeRotation;
                    }
                    break;
                case TransitionType.Exit:
                    character.StopCoroutine("UpEntranceRoutine");

                    if (forceFloatCoroutine != null)
                    {
                        character.StopCoroutine(forceFloatCoroutine);
                        forceFloatCoroutine = null;
                    }

                    if (m_exitDirection == TravelDirection.Up)
                    {
                        characterPhysics.velocity = Vector2.zero;
                        var exitVelocity = m_upVelocity;
                        exitVelocity.x *= m_forceExitFacing ? (int)m_exitFacing : (int)character.facing;
                        characterPhysics.AddForce(exitVelocity, ForceMode2D.Impulse);
                    }
                    else
                    {
                        characterPhysics.velocity = (Vector2.zero);
                    }
                    characterPhysics.constraints = RigidbodyConstraints2D.FreezeRotation;
                    break;
                case TransitionType.PostEnter:
                    characterPhysics.velocity = Vector2.zero;
                    characterPhysics.constraints = RigidbodyConstraints2D.FreezeRotation;

                    forceFloatCoroutine = character.StartCoroutine(ForceMidAirFloatRoutine(characterPhysics));
                    break;
                case TransitionType.PostExit:
                    GameplaySystem.playerManager.StopCharacterControlOverride();
                    break;
            }
        }

        private IEnumerator ForceMidAirFloatRoutine(Rigidbody2D physics)
        {
            while (true)
            {
                Debug.Log("test");
                physics.velocity = Vector2.zero;
                yield return null;
            }
        }

        private IEnumerator UpEntranceRoutine(Rigidbody2D physics)
        {
            var waitFor = new WaitForFixedUpdate();
            var time = transitionDelay;
            while (time > 0)
            {
                physics.velocity = Vector2.up * m_upVelocity;
                yield return waitFor;
                time -= Time.deltaTime;
            }
        }

#if UNITY_EDITOR
        [SerializeField, PropertyOrder(-1)]
        private bool m_customTravelDirections;

        private void OnDirectionChange()
        {
            if (m_customTravelDirections == false)
            {
                if (m_entranceDirection == TravelDirection.Up)
                {
                    m_exitDirection = TravelDirection.Down;
                }
                else
                {
                    m_exitDirection = TravelDirection.Up;
                }
            }
        }
#endif
    }
}
