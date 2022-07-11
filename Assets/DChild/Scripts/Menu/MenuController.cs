using DChild.Gameplay;
using Doozy.Engine.UI.Input;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Menu
{
    public class MenuController : MonoBehaviour
    {
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

        private void OnPause(InputValue value)
        {
            if (enabled == true)
            {
                var isTrue = value.Get<float>() == 1;
                if (isTrue)
                {
                    if (GameplaySystem.isGamePaused == false)
                    {
                        GameplaySystem.PauseGame();
                        GameplaySystem.gamplayUIHandle.ShowPauseMenu(true);
                        BackButton.Disable(); 
                    }
                    else
                    {
                        BackButton.EnableByForce();
                        //BackButton.Instance.Execute();
                    }
                }
            }
        }

        private void OnStore(InputValue value)
        {
            if (enabled == true)
            {
                var isTrue = value.Get<float>() == 1;
                if (isTrue)
                {
                    if (GameplaySystem.isGamePaused == false)
                    {
                        GameplaySystem.gamplayUIHandle.OpenStorePage();
                    }
                }
            }
        }

        private void Start()
        {
            m_enableStoreInput = true;
        }
    }
}