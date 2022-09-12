using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    [CreateAssetMenu(fileName = "MonsterCapsuleVisualData", menuName = "DChild/Gameplay/Environment/Monster Capsule Visual Data")]
    public class MonsterCapsuleVisualData : ScriptableObject
    {
        [System.Serializable]
        public class MonsterVisualInfo
        {
            [SerializeField]
            private Vector3 m_position;
            [SerializeField]
            private Vector3 m_scale;
            [SerializeField]
            private SkeletonDataAsset m_monsterSkeleton;
            [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_monsterSkeleton")]
            private string m_animation;

            public void LoadVisualsTo(SkeletonAnimation skeletonAnimation)
            {
                var transform = skeletonAnimation.transform;
                transform.localPosition = m_position;
                transform.localScale = m_scale;
                skeletonAnimation.skeletonDataAsset = m_monsterSkeleton;
                skeletonAnimation.Initialize(true);
                skeletonAnimation.AnimationName = m_animation;
            }
        }

        [SerializeField]
        private MonsterVisualInfo[] m_visualList;

        public int count => m_visualList.Length;

        public MonsterVisualInfo GetMonsterVisual(int index) => m_visualList[index];
    }
}
