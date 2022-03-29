using DChild.Visuals;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Visuals
{
    public class PlayerSpineSyncer : MonoBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField, SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_afterSyncAnimation;

        public void SyncWith(SpineSyncer reference)
        {
            Debug.Log("Player Syncer Test.");
            StopAllCoroutines();
            StartCoroutine(DelayedSync(reference));
        }

        private IEnumerator DelayedSync(SpineSyncer reference)
        {
            m_skeletonAnimation.state.SetAnimation(0, m_afterSyncAnimation, true);
            m_skeletonAnimation.AnimationState.GetCurrent(0).TrackTime = reference.skeleton.state.GetCurrent(0).TrackTime;
            yield return new WaitForEndOfFrame();
            var referenceTransform = reference.transform;
            transform.position = referenceTransform.position;
        }
    }
}
