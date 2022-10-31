using DChild.Temp;
using Doozy.Runtime.Signals;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace DChild.Gameplay.Systems
{

    public class GameplayInput : MonoBehaviour
    {
        [SerializeField]
        private InputSystemUIInputModule m_uiInput;
        [SerializeField]
        private InputActionReference m_storeToggleAction;
        [SerializeField]
        private bool m_actionIsCloseStore;

        private bool m_enableStoreInput;
        private bool m_inputOverridden;
        private InputActionReference m_uiMoveInput;

        public void ToggleUINavigationInput(bool On)
        {
            //m_uiInput.move = On ? m_uiMoveInput : null;
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
            m_uiMoveInput = m_uiInput.move;

            //Unity Input 1.4.X has a very weird bug This is a fix as advised by https://forum.unity.com/threads/input-system-1-4-1-released.1306062/
            InputSystem.settings.SetInternalFeatureFlag("DISABLE_SHORTCUT_SUPPORT", true);
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