using UnityEngine;

namespace DChild.Gameplay.Overworld
{
    [System.Serializable]
    public struct OverworldGridCalculator
    {
        [SerializeField]
        private Vector2 m_gridsize;
        [SerializeField]
        private float m_minXValue;
        [SerializeField]
        private float m_maxYValue;

        public OverworldGridCalculator(Vector2 gridsize, float minXValue, float maxYValue)
        {
            m_gridsize = gridsize;
            m_minXValue = minXValue;
            m_maxYValue = maxYValue;
        }

        public (int x, int y) GetGridIndex(Vector2 position) => (GetXIndex(position.x), GetYIndex(position.y));

        public int GetXIndex(float value) => GetIndex(value - m_minXValue, m_gridsize.x);

        public int GetYIndex(float value) => GetIndex(m_maxYValue - value, m_gridsize.y);

        public int GetIndex(float value, float modifier)
        {
            if (value == 0)
            {
                return 0;
            }
            else
            {
                return Mathf.FloorToInt(value / modifier);
            }
        }
    }

}