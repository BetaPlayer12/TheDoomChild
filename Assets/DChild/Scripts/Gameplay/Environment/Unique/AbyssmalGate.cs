using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public class AbyssmalGate : MonoBehaviour
    {
        [SerializeField]
        private Collider2D m_locationSwitchTrigger;
        [SerializeField]
        private GameObject m_tempVisuals;

        public void Open()
        {
            m_locationSwitchTrigger.enabled = true;
            m_tempVisuals.SetActive(true);
        }

        public void Close()
        {
            m_locationSwitchTrigger.enabled = false;
            m_tempVisuals.SetActive(false);
        }

        public void SetAsOpen()
        {
            m_locationSwitchTrigger.enabled = true;
            m_tempVisuals.SetActive(true);
        }

        public void SetAsClose()
        {
            m_locationSwitchTrigger.enabled = false;
            m_tempVisuals.SetActive(false);
        }
    }

}