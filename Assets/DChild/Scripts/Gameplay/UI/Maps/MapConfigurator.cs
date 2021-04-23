using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace DChild.Gameplay.UI.Map
{

    public class MapConfigurator : MonoBehaviour
    {
        [SerializeField, HorizontalGroup("Anchor"), OnValueChanged("UpdatePlayerTracker")]
        private Vector2 m_targetOrigin;
        [SerializeField, OnValueChanged("UpUpdatePlayerTrackerdatePosition")]
        private Vector2 m_anchorOrigin;
        [SerializeField, MinValue(0.1f), OnValueChanged("UpdatePlayerTracker")]
        private float m_toUIScale = 1;

        private void UpdatePlayerTracker()
        {

        }
        private void Start()
        {
            UpdatePlayerTracker();
        }

#if UNITY_EDITOR
        [ButtonGroup("Anchor/Button"), Button("UseCurrent")]
        private void UseCurrentAnchorPosition()
        {
            m_targetOrigin = transform.position;
        }
#endif
    }
}