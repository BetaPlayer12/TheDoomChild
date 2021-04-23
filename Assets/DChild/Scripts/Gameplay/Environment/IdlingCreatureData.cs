using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [CreateAssetMenu(fileName = "IdlingCreatureData", menuName = "DChild/Gameplay/Environment/Idling Creature Data")]
    public class IdlingCreatureData : ScriptableObject
    {
        [SerializeField]
        private SkeletonDataAsset m_skeletonData;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("@m_skeletonData != null")]
        private string[] m_animationList;

        public string[] animationList => m_animationList;
    }
}
