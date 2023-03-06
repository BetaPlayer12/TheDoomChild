using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.NavigationMap
{
    public class NavMapFogOfWarGroup : MonoBehaviour
    {
        [SerializeField, ListDrawerSettings(ShowIndexLabels = true, HideRemoveButton = true, HideAddButton = true)]
        private Image[] m_images;

#if UNITY_EDITOR
        public void RenameGameObject(string name, int index)
        {
            UpdateList();
            var objectName = $"{name}{index}";
            gameObject.name = objectName;

            for (int i = 0; i < m_images.Length; i++)
            {
                m_images[i].name = $"{objectName}_{i}";
            }
        }
#endif

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

        [ContextMenu("Update List")]
        private void UpdateList()
        {
            m_images = GetComponentsInChildren<Image>(true);
        }


        private void OnValidate()
        {
#if UNITY_EDITOR
            if (PrefabUtility.IsPartOfPrefabAsset(gameObject) == false)
            {
                UpdateList();
            }
#endif
        }
    }
}

