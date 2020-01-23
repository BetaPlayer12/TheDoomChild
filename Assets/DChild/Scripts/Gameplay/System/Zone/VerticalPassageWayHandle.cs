using Sirenix.Serialization;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [System.Serializable]
    public class VerticalPassageWayHandle : ISwitchHandle
    {
        private enum TravelDirection
        {
            Up,
            Down
        }

        [SerializeField]
        private TravelDirection m_entranceDirection;
        [SerializeField]
        private TravelDirection m_exitDirection;
        [SerializeField]
        private Vector2 m_upVelocity;

        public float transitionDelay => 1;

        public void DoSceneTransition(Character character, TransitionType type)
        {
            var characterPhysics = character.GetComponent<CharacterPhysics2D>();
            if (type == TransitionType.Enter)
            {
                if (m_entranceDirection == TravelDirection.Up)
                {
                    character.StartCoroutine(UpEntranceRoutine(characterPhysics));
                }
            }
            else
            {
                character.StopCoroutine("UpEntranceRoutine");
                if (m_exitDirection == TravelDirection.Up)
                {
                    characterPhysics.SetVelocity(Vector2.zero);
                    var exitVelocity = m_upVelocity;
                    exitVelocity.x *= (int)character.facing;
                    characterPhysics.AddForce(exitVelocity, ForceMode2D.Impulse);
                }
                else
                {
                    characterPhysics.SetVelocity(Vector2.zero);
                }
            }
        }

        private IEnumerator UpEntranceRoutine(CharacterPhysics2D physics)
        {
            var waitFor = new WaitForFixedUpdate();
            var time = transitionDelay;
            while (time > 0)
            {
                physics.SetVelocity(Vector2.up * m_upVelocity);
                yield return waitFor;
                time -= Time.deltaTime;
            }
        }
    }
}
