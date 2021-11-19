using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    [CreateAssetMenu(fileName = "ImmortalPoleGuyVisualData", menuName = "DChild/Gameplay/Environment/Visual Data/Immortal Pole Guy")]
    public class ImmortalPoleGuyVisualData : ScriptableObject, IVisualData
    {
        [System.Serializable]
        public class VisualInfo
        {
            [SerializeField]
            private SkeletonDataAsset m_skeleton;
            [SerializeField,Spine.Unity.SpineAnimation(dataField = "m_skeleton")]
            private string m_idleAnimation;
            [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeleton")]
            private string m_flinchAnimation;
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