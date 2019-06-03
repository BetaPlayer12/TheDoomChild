using DChild.Inputs;
using Holysoft.UI;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class PauseHandler : MonoBehaviour, IGameplaySystemModule
    {
        [SerializeField]
        private UICanvas m_pauseMenu;
        private bool m_isPaused;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (GameplaySystem.isGamePaused)
                {
                    GameplaySystem.ResumeGame();
                    m_pauseMenu.Hide();
                }
                else
                {
                    GameplaySystem.PauseGame();
                    m_pauseMenu.Show();
                }
            }
        }
    }
}