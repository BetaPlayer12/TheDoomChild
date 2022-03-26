using Spine.Unity;
using System.Collections;
using UnityEngine;

namespace DChild.Visuals
{
    public class SpineSyncer : MonoBehaviour
    {
        [SerializeField]
        private DChild.Gameplay.SpineAnimation m_animation;
        [SerializeField]
        private SkeletonAnimation m_skeleton;
        public SkeletonAnimation skeleton => m_skeleton;

        public void SyncWith(SpineSyncer reference)
        {
            StopAllCoroutines();
            StartCoroutine(DelayedSync(reference));
        }

        private IEnumerator DelayedSync(SpineSyncer reference)
        {
            if (m_animation != null)
            {
                var trackToReference = reference.skeleton.state.GetCurrent(0);
                m_animation.SetAnimation(0, trackToReference.Animation.Name, trackToReference.Loop);
                m_animation.animationState.GetCurrent(0).TrackTime = trackToReference.TrackTime;
                yield return new WaitForEndOfFrame();
                var referenceTransform = reference.transform;
                transform.position = referenceTransform.position;
            }
        }
    }

}