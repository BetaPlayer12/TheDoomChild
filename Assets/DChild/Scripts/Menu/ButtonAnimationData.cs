using UnityEngine;

namespace DChild.Menu.MainMenu
{
    [CreateAssetMenu(fileName = "ButtonAnimationData", menuName = "DChild/Menu/Animation Data/Button Animation")]
    public class ButtonAnimationData : ScriptableObject
    {
        [SerializeField]
        private Color m_nullFrameColor = new Color(0, 0, 0, 0);
        [SerializeField]
        private Color m_frameColor = Color.white;
        [SerializeField]
        private Sprite[] m_animation;

        public Color nullFrameColor => m_nullFrameColor;
        public Color frameColor => m_frameColor;
        public int frameCount => m_animation.Length;
        public Sprite GetFrame(int index) => m_animation[index];
    }
}