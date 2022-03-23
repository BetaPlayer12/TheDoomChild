/***********************************************
 * 
 * A Wrapper class for Skeleton Animation class
 * Contains functions that will make changing of Animation to be easy
 * 
 ***********************************************/

using System.Collections.Generic;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay
{
    public struct AnimationData
    {
        public AnimationData(int id, string name, bool loop) : this()
        {
            this.id = id;
            this.name = name;
            this.loop = loop;
            mixDuration = -1f;
        }

        public AnimationData(int id, string name, bool loop, float mixDuration) : this()
        {
            this.id = id;
            this.name = name;
            this.loop = loop;
            this.mixDuration = mixDuration;
        }

        public int id { get; }
        public string name { get; }
        public bool loop { get; }
        public float mixDuration { get; }
    }

    public struct AnimationEventArgs : IEventActionArgs
    {

        public AnimationEventArgs(AnimationData animationData) : this()
        {
            this.animationData = animationData;
        }

        public AnimationData animationData { get; }
    }

    public abstract class SpineAnimation : MonoBehaviour
    {
        /// <summary>
        /// This class is Use to Organize the animation of Large Spine Animation Collections
        /// </summary>
        public abstract class SubAnimation
        {
            protected SpineAnimation m_animation;
            public SubAnimation(SpineAnimation animation)
            {
                this.m_animation = animation;
            }
        }

        [System.Serializable]
        public class Track
        {
            [SerializeField]
            private int m_ID;
            [SerializeField]
            private string m_currentAnimation;

            public Track(int id)
            {
                m_ID = id;
                m_currentAnimation = "";
            }

            public int ID => m_ID;
            public string currentAnimation => m_currentAnimation;
            public void SetAnimation(string currentAnimation) => m_currentAnimation = currentAnimation;
        }

        private const string ANIMATION_EMPTY = "<empty>";

        [SerializeField]
        [ReadOnly]
        protected List<Track> m_animationTracks;
        private bool m_lockBehaviourWithAnimation;    //Make this true if Behaviour cannot be interrupted
        [SerializeField]
        [ReadOnly]
        protected SkeletonAnimation m_skeletonAnimation;

        public event EventAction<AnimationEventArgs> AnimationSet;

        public bool lockBehaviourWithAnimation => m_lockBehaviourWithAnimation;
        public SkeletonAnimation skeletonAnimation => m_skeletonAnimation;
        public Spine.AnimationState animationState => m_skeletonAnimation.AnimationState;
        public List<Track> animationTracks => m_animationTracks;

        public static bool IsAnEmptyAnimation(TrackEntry trackEntry) => trackEntry.Animation.Name == ANIMATION_EMPTY;

        public string GetCurrentAnimation(int id) => GetTrack(id).currentAnimation;

        public TrackEntry SetAnimation(int id, string name, bool loop, float mixDuration)
        {
            var trackEntry = SetAnimation(id, name, loop);
            trackEntry.MixDuration = mixDuration;
            var animationData = new AnimationData(id, name, loop, mixDuration);
            AnimationSet?.Invoke(this, new AnimationEventArgs(animationData));
            return trackEntry;
        }

        public TrackEntry SetAnimation(int id, string name, bool loop)
        {
            var track = GetTrack(id);
            if (track.currentAnimation == name)
            {
                return m_skeletonAnimation.state.GetCurrent(id);
            }
            else if (track.ID == -1) // new Track needed
            {
                var newTrack = new Track(id);
                newTrack.SetAnimation(name);
                m_animationTracks.Add(newTrack);
            }
            else
            {
                track.SetAnimation(name);
            }
            var animationData = new AnimationData(id, name, loop);
            AnimationSet?.Invoke(this, new AnimationEventArgs(animationData));
            return m_skeletonAnimation.state.SetAnimation(id, name, loop);
            //return null;
        }

        public TrackEntry AddAnimation(int id, string name, bool loop, float delay) => m_skeletonAnimation.state.AddAnimation(id, name, loop, delay);

        public TrackEntry SetEmptyAnimation(int trackIndex, float mixDuration) => m_skeletonAnimation.state.SetEmptyAnimation(trackIndex, mixDuration);

        public TrackEntry AddEmptyAnimation(int trackIndex, float mixDuration, float delay) => m_skeletonAnimation.state.AddEmptyAnimation(trackIndex, mixDuration, delay);

        public Track GetTrack(int id)
        {
            for (int i = 0; i < m_animationTracks.Count; i++)
            {
                if (m_animationTracks[i].ID == id)
                {
                    return m_animationTracks[i];
                }
            }
            return new Track(-1);
        }

        private void OnStart(TrackEntry trackEntry)
        {
            var track = GetTrack(trackEntry.TrackIndex);
            track.SetAnimation(trackEntry.Animation.Name);
            m_lockBehaviourWithAnimation = true;
        }

        private void OnComplete(TrackEntry trackEntry) => m_lockBehaviourWithAnimation = false;

        public void LateUpdateAnimation()
        {
            m_skeletonAnimation.LateUpdateAnimation();
        }

        public void UpdateAnimation(float deltaTime)
        {
            m_skeletonAnimation.Update(deltaTime);
        }

        protected virtual void Start()
        {
            m_skeletonAnimation.AnimationState.Start += OnStart;
            m_skeletonAnimation.AnimationState.Complete += OnComplete;
        }

        private void OnEnable()
        {
            if (SkeletonAnimationManager.hasInstance)
            {
                m_skeletonAnimation.enabled = false;
                SkeletonAnimationManager.Register(this);
            }
        }

        private void OnDisable()
        {
            if (SkeletonAnimationManager.hasInstance)
            {
                SkeletonAnimationManager.Unregister(this);
            }
        }

        protected void OnValidate()
        {
            m_skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        }
    }
}