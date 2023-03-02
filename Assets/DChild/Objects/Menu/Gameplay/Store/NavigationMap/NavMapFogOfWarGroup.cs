using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.NavigationMap
{
    public class NavMapFogOfWarGroup : MonoBehaviour
    {
        [SerializeField]
        private Image[] m_images;

        public void RevealArea(bool param)
        {
            for (int x = 0; x < m_images.Length; x++)
            {
                if (param == true)
                {
                    m_images[x].enabled = false;
                    
                }
                else
                {
                    m_images[x].enabled = true;
                }
                

            }     

        }
    }
}

