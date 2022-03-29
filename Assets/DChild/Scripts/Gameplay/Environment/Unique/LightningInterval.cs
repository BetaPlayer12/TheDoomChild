using UnityEngine;
using Holysoft.Collections;
using Sirenix.OdinInspector;
using DarkTonic.MasterAudio;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    public class LightningInterval : MonoBehaviour
    {
        [SerializeField]
        private Animator m_lightningAnimator;

        [SerializeField]
        private RangeFloat m_interval;

#if UNITY_EDITOR
        [SerializeField, PropertyOrder(0),ReadOnly]
        private int m_currentIndexPlayed;
#endif
        [SerializeField]
        private AnimationClip[] m_clips;
        [SerializeField, SoundGroup]
        private string[] m_matchingAudio;

        private float m_nextLightningInterval;
        private CallBackSounds m_callbackSounds;

        private void PlayRandomLightning()
        {
            var lightningIndex = Random.Range(0, m_clips.Length);
            var currentClip = m_clips[lightningIndex];

            m_lightningAnimator.Play(currentClip.name);
            m_callbackSounds.PlaySound(m_matchingAudio[lightningIndex]);
            m_nextLightningInterval = currentClip.length + m_interval.GenerateRandomValue();

#if UNITY_EDITOR
            m_currentIndexPlayed = lightningIndex;
#endif
        }

        private void Awake()
        {
            m_callbackSounds = GetComponent<CallBackSounds>();
        }

        private void OnEnable()
        {
            PlayRandomLightning();
        }

        private void LateUpdate()
        {
            m_nextLightningInterval -= GameplaySystem.time.deltaTime;
            if (m_nextLightningInterval <= 0)
            {
                PlayRandomLightning();
            }
        }
    }
}
