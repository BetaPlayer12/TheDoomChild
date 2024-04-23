using SSpineAnimation = Spine.Unity.SpineAnimation;
using UnityEngine;
using Spine.Unity;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PuedisYnnusSpike : MonoBehaviour
    {
        [SerializeField]
        private Collider2D m_collider;
        [SerializeField]
        private SpineRootAnimation m_animation;
        [SerializeField, SpineSkin]
        private List<string> m_variations;

        [SerializeField, SSpineAnimation]
        private string m_growAnimation;
        [SerializeField, SSpineAnimation]
        private string m_disappearSpawnAnimation;

        private SkeletonAnimation m_skeletonAnimation;

        public void Grow()
        {
            gameObject.SetActive(true);
           // RandomizeSkin();
            var animation = m_animation.SetAnimation(0, m_growAnimation, false);
            animation.MixDuration = 0;
            m_collider.enabled = true;
        }

        public void Disappear()
        {
            m_animation.SetAnimation(0, m_disappearSpawnAnimation, false);
            m_collider.enabled = false;
        }

        [Button]
        private void RandomizeSkin()
        {
            var chosenVariationIndex = UnityEngine.Random.Range(0, m_variations.Count);
            m_skeletonAnimation.skeleton.SetSkin(m_variations[chosenVariationIndex]);
        }

        private void Awake()
        {
            gameObject.SetActive(false);
            m_skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        }
    }
}