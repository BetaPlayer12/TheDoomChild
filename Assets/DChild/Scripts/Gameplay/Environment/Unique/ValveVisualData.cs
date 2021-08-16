using System;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [CreateAssetMenu(fileName = "ValveVisualData", menuName = "DChild/Gameplay/Environment/Valve Visual Data")]
    public class ValveVisualData : ScriptableObject
    {
        [System.Serializable]
        public class VisualInfo
        {
            [SerializeField]
            private Sprite m_valve;
            [SerializeField]
            private Sprite m_mechanism;

            public void LoadVisualsTo(SpriteRenderer valve, SpriteRenderer mechanism)
            {
                valve.sprite = m_valve;
                mechanism.sprite = m_mechanism;
            }
        }

        [SerializeField]
        private VisualInfo[] m_visualList;

        public int count => m_visualList.Length;

        public VisualInfo GetVisual(int index) => m_visualList[index];
    }
}