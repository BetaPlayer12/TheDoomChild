using DChild.Gameplay;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Menu
{
    public class PauseHandle : MonoBehaviour
    {
        [SerializeField]
        private ConfirmationRequestHandle m_returnToMainMenuRequest;

        public void PauseGame()
        {
            GameplaySystem.PauseGame();
        }

        public void ResumeGame()
        {
            GameplaySystem.ResumeGame();
        }

        public void BackToMainMenu()
        {
            m_returnToMainMenuRequest.Execute(OnMainMenuConfirm);
        }

        public void BackToMainMenuNoConfirmation()
        {
            //Intended for Demo purposes only
            if (GameplaySystem.campaignSerializer.slot.demoGame)
            {
                GameSystem.LoadMainMenu();
            }
        }

        private void OnMainMenuConfirm(object sender, EventActionArgs eventArgs)
        {
            GameSystem.LoadMainMenu();
        }
    }
}