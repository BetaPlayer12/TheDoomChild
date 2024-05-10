using DChild.Gameplay.Characters;
using SSpineAnimation = Spine.Unity.SpineAnimation;
using UnityEngine;
using Spine.Unity;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters.Enemies
{

    public class PuedisYnnusSpikeBeat : MonoBehaviour
    {
        private enum GrowthStage
        {
            Disappear,
            Half,
            Full,
        }

        [SerializeField]
        private Collider2D m_collider;
        [SerializeField]
        private SpineRootAnimation m_animation;
        [SerializeField, SpineSkin]
        private List<string> m_variations;


        [SerializeField, SSpineAnimation]
        private string m_halfSpawnAnimation;
        [SerializeField, SSpineAnimation]
        private string m_fullSpawnAnimation;
        [SerializeField, SSpineAnimation]
        private string m_disappearSpawnAnimation;

        private GrowthStage m_currentGrowthStage;
        private SkeletonAnimation m_skeletonAnimation;

        public void ProgressGrowth()
        {
            switch (m_currentGrowthStage)
            {
                case GrowthStage.Disappear:
                    if (gameObject.activeInHierarchy == false)
                    {
                        gameObject.SetActive(true);
                    }
                    m_currentGrowthStage = GrowthStage.Half;
                    break;
                case GrowthStage.Half:
                    m_currentGrowthStage = GrowthStage.Full;
                    RandomizeSkin();
                    break;
                case GrowthStage.Full:
                    m_currentGrowthStage = GrowthStage.Disappear;
                    break;
            }

            switch (m_currentGrowthStage)
            {
                case GrowthStage.Half:
                    m_animation.SetAnimation(0, m_halfSpawnAnimation, false);
                    m_collider.enabled = false;
                    break;
                case GrowthStage.Full:
                    m_animation.SetAnimation(0, m_fullSpawnAnimation, false);
                    m_collider.enabled = true;
                    break;
                case GrowthStage.Disappear:
                    m_animation.SetAnimation(0, m_disappearSpawnAnimation, false);
                    m_collider.enabled = false;
                    break;
            }
        }

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