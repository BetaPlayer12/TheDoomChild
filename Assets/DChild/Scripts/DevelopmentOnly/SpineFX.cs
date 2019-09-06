using Sirenix.OdinInspector;
using USpine = Spine.Unity;
using Spine.Unity;
using UnityEngine;
using Spine;
using System;
using DChild.Gameplay.Characters;

namespace DChild.Gameplay
{
    public class SpineFX : FX
    {
        [SerializeField]
        [Spine.Unity.SpineAnimation]
        private string m_playFX;
        [SerializeField]
        private bool m_isLooping;
        private SkeletonAnimation m_spine;

        private bool m_shouldReplay;

        [Button("Play")]
        public override void Play()
        {
            if (m_shouldReplay)
            {
                var trackEntry = m_spine.AnimationState.SetAnimation(0, m_playFX, m_isLooping);
                trackEntry.MixDuration = 0;
            }
        }

        public override void Stop()
        {
            m_spine.AnimationState.SetEmptyAnimation(0, 0);
<<<<<<< HEAD
=======
            CallFXDone();
            CallPoolRequest();
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
        }

        public override void Pause()
        {
            //Dunno How to really
            throw new NotImplementedException();
        }

        public override void SetFacing(HorizontalDirection direction) => m_spine.skeleton.FlipX = direction == HorizontalDirection.Left;

        private void OnComplete(TrackEntry trackEntry)
        {
            CallFXDone();
            CallPoolRequest();
        }

        private void Awake()
        {
            m_spine = GetComponentInChildren<SkeletonAnimation>();
            m_shouldReplay = false;
        }

        private void Start()
        {
            if (m_isLooping == false)
            {
                m_spine.AnimationState.Complete += OnComplete;
            }
        }

        private void OnDisable()
        {
            m_shouldReplay = true;
        }


    }

}