using DChild.Gameplay.Characters;
using DChild.Gameplay.Projectiles;
using Holysoft.Event;
using Spine;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters
{
    [System.Serializable]
    public class AttackHandle
    {
        [SerializeField]
        private SpineRootAnimation m_animation;
        private Spine.TrackEntry m_cacheTrack;
        public event EventAction<EventActionArgs> AttackDone;

        public void ExecuteAttack(string attackAnimation)
        {
            if (m_cacheTrack != null)
            {
                m_cacheTrack.Complete -= OnTrackDone;
                m_cacheTrack.Interrupt -= OnTrackDone;
            }

            m_cacheTrack = m_animation.SetAnimation(0, attackAnimation, false);
            m_cacheTrack.MixDuration = 0;
            m_cacheTrack.Complete += OnTrackDone;
            m_cacheTrack.Interrupt += OnTrackDone;
        }

        private void OnTrackDone(TrackEntry trackEntry)
        {
            if (m_cacheTrack == trackEntry)
            {
                AttackDone?.Invoke(this, EventActionArgs.Empty);
            }
        }
    }
}