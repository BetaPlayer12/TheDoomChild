using DChild.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerIntroFunctions : MonoBehaviour
    {
        [SerializeField]
        private List<IntroActions> allowedActions;
        [SerializeField]
        private UnityEvent m_forceInteract;

        private (Damageable damageable, Character character) m_targetTuple;

        public void EnableIntroControls()
        {
            GameplaySystem.playerManager.EnableIntroControls();
        }

        public void DisableIntroControls()
        {
            GameplaySystem.playerManager.DisableIntroControls();
        }

        public void EnabledAllowedAction()
        {
            GameplaySystem.playerManager.EnableIntroAction(allowedActions);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != "Sensor")
            {
                var target = collision.GetComponentInParent<ITarget>();
                if (target.CompareTag(Character.objectTag))
                {
                    m_targetTuple = (collision.GetComponentInParent<Damageable>(), collision.GetComponentInParent<Character>());
                    m_forceInteract.Invoke();
                }
            }
        }
    }
}