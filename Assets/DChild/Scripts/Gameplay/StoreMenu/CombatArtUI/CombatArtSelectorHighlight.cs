using UnityEngine;

namespace DChild.Gameplay.UI.CombatArts
{
    public class CombatArtSelectorHighlight : MonoBehaviour
    {
        [SerializeField]
        private RectTransform m_highlightRect;

        private Transform m_originalHighlightRectParent;

        public void Highlight(CombatArtSelectButton combatArtSelectButton)
        {
            m_highlightRect.SetParent(combatArtSelectButton.transform);
            m_highlightRect.anchoredPosition = Vector2.zero;
            m_highlightRect.SetParent(m_originalHighlightRectParent);
        }

        public void Initialize()
        {
            m_originalHighlightRectParent = m_highlightRect.parent;
        }
    }

}