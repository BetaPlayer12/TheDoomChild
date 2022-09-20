using DChild.Temp;
using Doozy.Runtime.Signals;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Gameplay.Systems
{

    public class GameplayInput : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference m_storeToggleAction;
        [SerializeField]
        private bool m_actionIsCloseStore;

        private bool m_enableStoreInput;
        private bool m_inputOverridden;

        public void Disable()
        {
            enabled = false;
        }

        public void Enable()
        {
            enabled = true;
        }

        public void OverrideNewInfoNotif(float duration)
        {
            StopAllCoroutines();
            StartCoroutine(OverrideStoreOpen(duration));
        }

        public void SetStoreInputActive(bool isActive)
        {
            m_enableStoreInput = isActive;
        }

        private IEnumerator OverrideStoreOpen(float duration)
        {
            m_inputOverridden = true;
            yield return new WaitForSeconds(duration);
            m_inputOverridden = false;
        }

        public void SetStoreToggleAction(bool closeStoreOnAction)
        {
            m_actionIsCloseStore = closeStoreOnAction;
        }

        private void OnOpenStoreAction(InputAction.CallbackContext obj)
        {
            if (m_actionIsCloseStore)
            {
                //GameplaySystem.gamplayUIHandle.CloseStorePage();
            }
            else
            {
                if (m_inputOverridden)
                {
                    GameplaySystem.gamplayUIHandle.PromptJournalUpdateNotification();
                }
                else
                {
                    GameplaySystem.gamplayUIHandle.OpenStore();
                }
            }
        }


        private void Awake()
        {
            m_storeToggleAction.action.performed += OnOpenStoreAction;
        }


#if UNITY_EDITOR
        [Button, HideInEditorMode]
        private void SimulateOverride()
        {
            GameplaySystem.gamplayUIHandle.ShowJournalNotificationPrompt(3);
            OverrideNewInfoNotif(3);
        }
#endif
    }
}