using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class WaterfallFlowHandle : MonoBehaviour
    {
        [SerializeField]
        private Waterfall[] m_waterfallSegment;

        private int m_currentSegmentIndex;

        public void BlockWater()
        {
            m_waterfallSegment[m_currentSegmentIndex].StopFlow();
        }

        public void UnblockWater()
        {
            m_currentSegmentIndex = (int)Mathf.Repeat(m_currentSegmentIndex + 1, m_waterfallSegment.Length - 1);
            m_waterfallSegment[m_currentSegmentIndex].StartFlow();
        }
    }
}
