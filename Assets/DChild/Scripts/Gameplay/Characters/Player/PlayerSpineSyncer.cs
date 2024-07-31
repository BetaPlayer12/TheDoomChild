using DChild.Gameplay;
using Spine.Unity;
using System.Collections;
using UnityEngine;

namespace DChild.Visuals
{
    public class PlayerSpineSyncer : MonoBehaviour
    {
        [SerializeField]
        private Character m_character;
        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_afterSyncAnimation;

        public void SyncWith(SpineSyncer reference)
        {
            StopAllCoroutines();
            StartCoroutine(DelayedSync(reference));
        }

        public void SyncImmidiateWith(SpineSyncer reference)
        {
            StopAllCoroutines();
            SyncAnimation(reference);
            SyncPosition(reference);
        }

        private IEnumerator DelayedSync(SpineSyncer reference)
        {
            SyncAnimation(reference);
            yield return new WaitForEndOfFrame();
            SyncPosition(reference);

            //if ((int)m_character.facing != reference.transform.localScale.x)
            //{
            //    reference.transform.localScale = new Vector3((int)m_character.facing, reference.transform.localScale.y, reference.transform.localScale.z);
            //}
        }

        private void SyncPosition(SpineSyncer reference)
        {
            var referenceTransform = reference.transform;
            transform.position = referenceTransform.position;
            transform.localScale = referenceTransform.localScale;
        }

        private void SyncAnimation(SpineSyncer reference)
        {
            m_skeletonAnimation.state.SetAnimation(0, m_afterSyncAnimation, true);
            m_skeletonAnimation.AnimationState.GetCurrent(0).TrackTime = reference.skeleton.state.GetCurrent(0).TrackTime;
        }
    }
}
