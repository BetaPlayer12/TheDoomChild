using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.UI
{
    public class SkeletonGraphicControllerGroup : MonoBehaviour
    {
        [SerializeField]
        private bool m_getAllActiveChildren;
        [SerializeField, DisableIf("m_getAllActiveChildren")]
        private SkeletonGraphicController[] m_group;

        public void Play()
        {
            for (int i = 0; i < m_group.Length; i++)
            {
                m_group[i].Play();
            }
        }

        public void Stop()
        {
            for (int i = 0; i < m_group.Length; i++)
            {
                m_group[i].Stop();
            }
        }

        public void Freeze()
        {
            for (int i = 0; i < m_group.Length; i++)
            {
                m_group[i].Freeze();
            }
        }

        public void Unfreeze()
        {
            for (int i = 0; i < m_group.Length; i++)
            {
                m_group[i].Unfreeze();
            }
        }

        public void Reset()
        {
            for (int i = 0; i < m_group.Length; i++)
            {
                m_group[i].Reset();
            }
        }

        private void Awake()
        {
            if (m_getAllActiveChildren)
            {
                m_group = GetComponentsInChildren<SkeletonGraphicController>();
            }
        }
    }
}