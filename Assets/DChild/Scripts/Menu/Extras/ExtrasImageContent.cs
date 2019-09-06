using DChild.Menu.Extras;
using UnityEngine;

namespace Refactor.DChild.Menu.Extras
{
    public class ExtrasImageContent : MonoBehaviour
    {
        [SerializeField]
        private SpriteList m_list;
        [SerializeField]
        private ExtrasImageShowcase m_showcase;

        private void Awake()
        {
            var items = GetComponentsInChildren<ExtrasItem>();
            for (int i = 0; i < items.Length; i++)
            {
                items[i].Selected += OnItemSelected;
            }
        }

        private void OnItemSelected(object sender, ItemSelectedEventArgs eventArgs)
        {
            m_showcase.Showcase(m_list, eventArgs.itemIndex);
        }
    }
}