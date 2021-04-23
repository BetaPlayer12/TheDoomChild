/***********************************************
 * 
 * Base Animation class for All Character Animations
 * Should Contain functions that will be present to all
 * Types of Characters
 * 
 * Child Classess should have functions that does not care about 
 * Transistion from one state to another
 * 
 ***********************************************/

using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.Characters
{
    [RequireComponent(typeof(SpineAnimation))]
    public class SpineAnimationCaller : MonoBehaviour
    {
        private SpineAnimation m_animation;
        private bool m_isLooping;
        private int m_trackIndex;

        public void SetLoop(bool isLooping)
        {
            m_isLooping = isLooping;
        }

        public void SetTrackIndex(int index)
        {
            m_trackIndex = index;
        }

        public void SetAnimation(AnimationReferenceAsset animation)
        {
            m_animation.SetAnimation(m_trackIndex, animation.Animation.Name, m_isLooping);
        }

        private void Awake()
        {
            m_animation = GetComponent<SpineAnimation>();
        }
    }
}