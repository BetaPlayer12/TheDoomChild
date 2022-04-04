using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerIntroFunctions : MonoBehaviour
    {
        [SerializeField]
        private List<IntroActions> allowedActions;

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
    }
}