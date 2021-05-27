using Cinemachine;
using DChild.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChildDebug.Window
{

    public class ToggleDetachCamera : MonoBehaviour
    {
        [SerializeField]
        private DetachCameraControls m_controller;
        private Camera m_detachable;

        [Button]
        public void ToggleOn()
        {
            m_detachable = GameplaySystem.cinema.mainCamera;
            m_detachable.GetComponent<CinemachineBrain>().enabled = false;
            GameplaySystem.playerManager.OverrideCharacterControls();
            m_controller.enabled = true;
        }

        [Button]
        public void ToggleOff()
        {
            m_detachable = GameplaySystem.cinema.mainCamera;
            m_detachable.GetComponent<CinemachineBrain>().enabled = true;
            GameplaySystem.playerManager.StopCharacterControlOverride();
            m_controller.enabled = false;
        }
       
    }
}
