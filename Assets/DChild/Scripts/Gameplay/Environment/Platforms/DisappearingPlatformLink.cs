using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class DisappearingPlatformLink : MonoBehaviour
    {
        [SerializeField, TabGroup("Enter")]
        private UnityEvent m_onEnter;
        [SerializeField, TabGroup("Exit")]
        private UnityEvent m_onExit;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
            if (playerObject != null)
            {

                m_onEnter?.Invoke();
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
            if (playerObject != null)
            {

                m_onExit?.Invoke();
            }
        }


    }
}
