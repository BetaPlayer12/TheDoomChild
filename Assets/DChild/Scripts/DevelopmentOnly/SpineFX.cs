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
        private SkeletonAnimation m_spine;

        private bool m_shouldReplay;

        [Button("Play")]
        public override void Play()
        {
            if (m_shouldReplay)
            {
                var trackEntry = m_spine.AnimationState.SetAnimation(0, m_playFX, false);
                trackEntry.MixDuration = 0;
            }
        }

        public void SetFacing(HorizontalDirection direction) => m_spine.skeleton.FlipX = direction == HorizontalDirection.Left;

        private void OnComplete(TrackEntry trackEntry)
        {
            GameSystem.poolManager.GetOrCreatePool<FXPool>().AddToPool(this);
        }

        private void Awake()
        {
            m_spine = GetComponentInChildren<SkeletonAnimation>();
            m_shouldReplay = false;
        }

        private void Start()
        {
            m_spine.AnimationState.Complete += OnComplete;
        }

        private void OnDisable()
        {
            m_shouldReplay = true;
        }

        private void OnValidate()
        {
            FXValidate();
        }
    }

}