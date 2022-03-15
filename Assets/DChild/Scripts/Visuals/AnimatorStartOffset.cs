using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Visuals
{
    public class AnimatorStartOffset : MonoBehaviour
    {
        [SerializeField]
        private bool m_alwaysRandom;
        [SerializeField, MinValue(0), HideIf("m_alwaysRandom"), MaxValue("m_animationDuration")]
        private float m_startTime;

#if UNITY_EDITOR
        [Title("Editor Stuff")]
        [SerializeField]
        private AnimationClip m_referenceClip;
        [SerializeField, ReadOnly]
        private float m_animationDuration;

        [Button]
        private void RandomizeStartTime()
        {
            m_startTime = Random.Range(0, m_animationDuration);
        }

        private void UpdateAnimationDuration()
        {
            if (m_referenceClip == null)
            {
                m_animationDuration = 0;
            }
            else
            {
                m_animationDuration = m_referenceClip.length;
            }
        }
#endif

        void Start()
        {
            var animator = GetComponent<Animator>();
            if (m_alwaysRandom)
            {
                var startTime = Random.Range(0, animator.GetCurrentAnimatorStateInfo(0).length);
                animator.playbackTime = m_startTime;
            }
            else
            {
                animator.playbackTime = m_startTime;

            }
        }
    }

}