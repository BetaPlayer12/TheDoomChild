using Doozy.Engine;
using Sirenix.OdinInspector;
using System.Collections;
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
        private bool m_inputOverridden;


        public void Disable()
        {
            m_enableInput = false;
        }

        public void Enable()
        {
            m_enableInput = true;
        }

        public void OverrideNewInfoNotif(float duration)
        {
            StopAllCoroutines();
            StartCoroutine(OverrideStoreOpen(duration));
        }

        private IEnumerator OverrideStoreOpen(float duration)
        {
            m_inputOverridden = true;
            yield return new WaitForSeconds(duration);
            m_inputOverridden = false;
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
                GameplaySystem.gamplayUIHandle.ShowPauseMenu(true);
            }
            else if (m_enableInput == true)
            {
                if (Input.GetKeyDown(m_storeOpen))
                {
                    if (m_inputOverridden)
                    {
                        GameplaySystem.gamplayUIHandle.PromptJournalUpdateNotification();
                    }
                    else
                    {
                        GameplaySystem.gamplayUIHandle.OpenStorePage();
                    }
                }
            }
        }

#if UNITY_EDITOR
        [Button,HideInEditorMode]
        private void SimulateOverride()
        {
            GameplaySystem.gamplayUIHandle.ShowJournalNotificationPrompt(3);
            OverrideNewInfoNotif(3);
        }
#endif
    }
}