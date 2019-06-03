using Holysoft;
using Holysoft.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class StoreMenu : MonoBehaviour
    {
        [SerializeField]
        private UICanvas m_canvas;
        [SerializeField]
        private StoreNavigation m_navigation;

        public void OpenAt(StorePage page)
        {
            GameplaySystem.PauseGame();
            m_canvas.Show();
            m_navigation.Open(page);
        }

        public void Close()
        {
            m_canvas.Hide();
            GameplaySystem.ResumeGame();
        }

        private void OnValidate()
        {
            ComponentUtility.AssignNullComponent(this, ref m_canvas);
            ComponentUtility.AssignNullComponent(this, ref m_navigation);
        }
    }
}