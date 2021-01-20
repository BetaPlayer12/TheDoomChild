using Doozy.Engine;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class GameplayInput : MonoBehaviour
    {
        [SerializeField]
        private KeyCode m_pause;
        [SerializeField]
        private KeyCode m_storeOpen;


        private bool m_enableInput;
        private static GameplayUIHandle m_gameplayUIHandle;

        public void Disable()
        {
            m_enableInput = false;
        }

        public void Enable()
        {
            m_enableInput = true;
        }

        private void Awake()
        {
            m_enableInput = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(m_pause))
            {
                GameplaySystem.PauseGame();
                m_gameplayUIHandle.ShowPauseMenu(true);
            }
            else if(m_enableInput == true)
            {
                if (Input.GetKeyDown(m_storeOpen))
                {
                    GameplaySystem.gameplayModeHandle.OpenStorePage();
                }
            }
        }
    }
}