using DChild.Gameplay;
using UnityEngine;

namespace DChild
{
    public class DemoGuide : MonoBehaviour
    {
        public void PauseGame()
        {
            GameplaySystem.PauseGame();
        }
        public void ResumeGame()
        {
            GameplaySystem.ResumeGame();
        }
    }

}