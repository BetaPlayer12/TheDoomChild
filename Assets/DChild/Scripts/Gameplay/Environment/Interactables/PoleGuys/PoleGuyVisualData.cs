using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    [CreateAssetMenu(fileName = "PoleGuyVisualData", menuName = "DChild/Gameplay/Environment/Pole Guy Visual Data")]
    public class PoleGuyVisualData : ScriptableObject
    {
        [System.Serializable]
        public class VisualInfo
        {
            [SerializeField]
            private SkeletonDataAsset m_skeleton;
            [SerializeField]
            private Vector2 m_colliderOffset;
            [SerializeField]
            private Vector2 m_colliderSize;

            public void LoadVisualsTo(SkeletonAnimation skeletonAnimation, BoxCollider2D collider)
            {
                skeletonAnimation.skeletonDataAsset = m_skeleton;
               skeletonAnimation.Initialize(true);

                collider.offset = m_colliderOffset;
                collider.size = m_colliderSize;
            }
        }

        [SerializeField]
        private VisualInfo[] m_visualList;

        public int count => m_visualList.Length;

        public VisualInfo GetVisualInfo(int index) => m_visualList[index];
    }
}