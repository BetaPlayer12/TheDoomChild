using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    public class MapPointOfInterestTracker : MonoBehaviour
    {
        [SerializeField, VariablePopup(true)]
        private string m_varName;
        [ShowInInspector, HideInPrefabAssets]
        private bool m_isTracked = true;
        private Collider2D m_trigger;

        //public event EventAction<FogOfWarStateChangeEvent> RevealValueChange;

        public string varName => m_varName;
        public bool isTracked => m_isTracked;

        public void SetState(bool isRevealed)
        {
            SetStateAs(isRevealed);
            //RevealValueChange?.Invoke(this, new FogOfWarStateChangeEvent(m_varName, m_isRevealed));
        }

        public void SetStateAs(bool isRevealed)
        {
            m_isTracked = isRevealed;
        }
    }
}
