using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class CompositeIllusionPlatform : IllusionPlatform
    {
        [SerializeField]
        private IllusionPlatform[] m_list;

#if UNITY_EDITOR
        public IllusionPlatform[] list => m_list;
#endif

        public override void Appear(bool instant)
        {
            for (int i = 0; i < m_list.Length; i++)
            {
                m_list[i].Appear(instant);
            }
        }

        public override void Disappear(bool instant)
        {
            for (int i = 0; i < m_list.Length; i++)
            {
                m_list[i].Disappear(instant);
            }
        }
    }
}
