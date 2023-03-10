using System;
using Holysoft.Collections;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    public class NavigationMapInstance : MonoBehaviour
    {
        [SerializeField]
        private NavMapFogOfWarUI[] m_sceneFogOFWars;

        public NavMapFogOfWarUI GetFogOfWarOfScene(int index) => m_sceneFogOFWars[index];

        public void UpdateUI()
        {
            for (int i = 0; i < m_sceneFogOFWars.Length; i++)
            {
                m_sceneFogOFWars[i].UpdateUI();
            }
        }

        public void SetUIState(string fogOfWarName, Flag flag)
        {
            for (int i = 0; i < m_sceneFogOFWars.Length; i++)
            {
                var fogOfWar = m_sceneFogOFWars[i];
                if (fogOfWar.HasID(fogOfWarName))
                {
                    fogOfWar.SetUIState(fogOfWarName, flag);
                    return;
                }
            }
        }
    }
}