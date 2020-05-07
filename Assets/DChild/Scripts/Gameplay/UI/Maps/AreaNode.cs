using DChild.Gameplay.Environment;
using UnityEngine;

namespace DChild.Gameplay.UI.Map
{
    public class AreaNode : MonoBehaviour
    {
        [SerializeField]
        private Location m_location;

        public Location location => m_location;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnValidate()
        {
            gameObject.name = m_location.ToString().Replace("_", string.Empty) + "Node";
        }
    }
}