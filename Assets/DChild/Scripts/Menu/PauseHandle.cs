using System;
using DChild.Gameplay;
using Doozy.Engine.UI.Input;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Menu
{
    public class PauseHandle : MonoBehaviour
    {
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
            if (GameSystem.RequestConfirmation(OnMainMenuConfirm, "Do you want to return to main menu") == false)
            {
                LoadingHandle.SetLoadType(LoadingHandle.LoadType.Force);
                GameSystem.LoadMainMenu();
            }
            //GameSystem.LoadMainMenu();
        }

        private void OnMainMenuConfirm(object sender, EventActionArgs eventArgs)
        {
            GameSystem.LoadMainMenu();
        }
    }
}