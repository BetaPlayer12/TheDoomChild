using DChild.Gameplay;
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
            GameSystem.LoadMainMenu();
        }
    }
}