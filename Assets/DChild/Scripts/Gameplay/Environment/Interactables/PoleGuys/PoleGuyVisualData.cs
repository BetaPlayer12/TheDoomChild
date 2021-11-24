using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    [CreateAssetMenu(fileName = "PoleGuyVisualData", menuName = "DChild/Gameplay/Environment/Visual Data/Pole Guy")]
    public class PoleGuyVisualData : ScriptableObject, IVisualData
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
                if (skeletonAnimation.initialFlipX)
                {
                  
                    collider.offset = new Vector2(collider.offset.x * -1, collider.offset.y);
                }
                    
                collider.size = m_colliderSize;
            }
        }

        [SerializeField, ListDrawerSettings(ShowIndexLabels = true,IsReadOnly = true)]
        private VisualInfo[] m_visualList;

        public int count => m_visualList.Length;

        public VisualInfo GetVisualInfo(int index) => m_visualList[index];
    }
}