using UnityEngine;
using Spine.Unity;
using DarkTonic.MasterAudio;

namespace DChild
{
    [RequireComponent(typeof(AmbientSound))]
    public class SpineMatchedAmbientSound : MonoBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_matchWith;
        [SerializeField]
        private bool m_matchUsingPercentageTime;

        private AmbientSound m_ambientSound;

        private bool m_wasPlayingInLastFrame;

        private void MatchSoundWithAnimation(AudioSource audioSource)
        {
            if (m_matchUsingPercentageTime)
            {
                var currentAnimation = m_matchWith.state.GetCurrent(0);
                var percentTime = currentAnimation.AnimationTime / currentAnimation.Animation.Duration;
                audioSource.time = audioSource.clip.length * percentTime;
            }
            else
            {
            audioSource.time = m_matchWith.state.GetCurrent(0).AnimationTime;
            }
        }

        private void Start()
        {
            m_ambientSound = GetComponent<AmbientSound>();
        }

        private void LateUpdate()
        {
            var isPlaying = MasterAudio.IsTransformPlayingSoundGroup(m_ambientSound.AmbientSoundGroup, transform);
            if(m_wasPlayingInLastFrame != isPlaying)
            {
                if (isPlaying)
                {
                    MatchSoundWithAnimation(MasterAudio.GetAllPlayingVariationsOfTransform(transform)[0].VarAudio);
                }
                m_wasPlayingInLastFrame = isPlaying;
            }
        }
    }
}