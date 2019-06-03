using Spine.Unity;
using UnityEngine;


namespace DChild.Gameplay.Systems.WorldComponents
{
    public class WorldSpineAnimations : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        private SpineObjects m_spineAnimations;

        private void OnEnable()
        {
            if (m_spineAnimations != null)
            {
                GameplaySystem.world.Register(m_spineAnimations);
            }
        }

        private void OnDisable()
        {
            if (m_spineAnimations != null)
            {
                GameplaySystem.world.Unregister(m_spineAnimations);
            }
        }

        private void OnValidate()
        {
            var skeletonAnimations = GetComponentsInChildren<SkeletonAnimation>();
            if(skeletonAnimations == null)
            {
                m_spineAnimations = null;
            }
            else
            {
                m_spineAnimations = new SpineObjects(skeletonAnimations);
            }
        }
    }
}