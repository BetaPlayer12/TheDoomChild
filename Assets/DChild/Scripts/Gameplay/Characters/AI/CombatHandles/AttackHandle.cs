using Holysoft.Event;
using Spine;
using UnityEngine;

namespace DChild.Gameplay.Characters
{
    [System.Serializable]
    public class AttackHandle
    {
        [SerializeField]
        private SpineRootAnimation m_animation;
        private Spine.TrackEntry m_cacheTrack;
        public event EventAction<EventActionArgs> AttackDone;

        public void ExecuteAttack(string attackAnimation, string idleAnimation)
        {
            if (m_cacheTrack != null)
            {
                m_cacheTrack.Complete -= OnTrackDone;
                m_cacheTrack.Interrupt -= OnTrackDone; //Commented dis to fix the transitional issue of attacks
            }
            m_cacheTrack = m_animation.SetAnimation(0, attackAnimation, false, 0);
            m_cacheTrack.MixDuration = 0;
            m_cacheTrack.Complete += OnTrackDone;
            m_cacheTrack.Interrupt += OnTrackDone;
            //m_animation.AddEmptyAnimation(0, 0, 0); //Commented dis to fix the transitional issue of attacks
            if (idleAnimation != null)
            {
                m_animation.AddAnimation(0, idleAnimation, true, 0);
            }
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