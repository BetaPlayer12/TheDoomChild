using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Menu.UI
{
    public class ChildUIContainer : MonoBehaviour
    {
        [SerializeField]
        private UIContainer m_parentContainer;
        private UIContainer m_self;

        private void Awake()
        {
            m_self = GetComponent<UIContainer>();
            m_parentContainer.OnShowCallback.Event.AddListener(() => m_self.Show());
            m_parentContainer.OnHideCallback.Event.AddListener(() => m_self.Hide());
        }
    }

}