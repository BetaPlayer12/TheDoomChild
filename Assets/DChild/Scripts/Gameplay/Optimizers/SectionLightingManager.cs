using DChild.Gameplay.Cinematics;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Optimization.Lights
{
    public class SectionLightingManager : MonoBehaviour
    {
        public static SectionLightingManager instance { get; private set; }


        private List<SectionLighting> m_list;
        private CameraBounds m_currentCamerBounds;

        public void Register(SectionLighting instance)
        {
            m_list.Add(instance);
        }

        public void Unregister(SectionLighting instance)
        {
            m_list.Remove(instance);
        }

        private void OnMainCameraChange(Camera obj)
        {
            m_currentCamerBounds = obj.GetComponent<CameraBounds>();
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }

            m_list = new List<SectionLighting>();
            GameplaySystem.cinema.OnMainCameraChange += OnMainCameraChange;
        }

        private void Update()
        {
            if (m_currentCamerBounds == null)
                return;

            for (int i = 0; i < m_list.Count; i++)
            {
                var sectionLight = m_list[i];
                if (sectionLight.IsVisible(m_currentCamerBounds))
                {
                    sectionLight.EnableAllLights();
                }
                else
                {
                    sectionLight.DisableAllLigts();
                }
            }
        }
    }

}