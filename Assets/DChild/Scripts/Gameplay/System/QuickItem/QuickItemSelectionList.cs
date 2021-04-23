using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Inventories.QuickItem
{
    public class QuickItemSelectionList : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_template;
        [SerializeField]
        private RectTransform m_parent;

        private List<QuickItemSelectionElement> m_elements;

        public void UpdateElements(IItemContainer itemContainer)
        {

        }
    }
}
