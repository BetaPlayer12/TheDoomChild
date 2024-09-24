using DChild.Gameplay.Cinematics.Cameras;
using UnityEngine;
using Sirenix.Serialization.Utilities;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;
using Cinemachine;

namespace DChild.Gameplay.Cinematics
{
    [System.Serializable]
    public class CameraShakeBlendHandle
    {
        [System.Serializable]
        private class ShakeClip
        {
            private int m_executionID;

            [ShowInInspector, ProgressBar(0, "duration")]
            private float m_timer;
            [ShowInInspector]
            private CameraShakeData m_data;

            public CameraShakeInfo info => m_data.cameraShakeInfo;
            public float amplitude => info.GetAmplitude(m_timer);
            public float frequency => info.GetFrequency(m_timer);

            [ShowInInspector]
            public int priority => info.priority;
            [ShowInInspector]
            public int executionID => m_executionID;

            public bool hasEnded => m_timer >= info.duration;

            private float duration => info.duration;

            public void Initialize(CameraShakeData data, int executionID)
            {
                m_timer = 0;
                m_data = data;
                m_executionID = executionID;
            }

            public void UpdateTimer(float delta)
            {
                m_timer += delta;
            }
        }

        [ShowInInspector, DisableInPlayMode, HideInEditorMode]
        private List<Cache<ShakeClip>> m_queue;

        private Comparison<Cache<ShakeClip>> m_queueComparison;
        private int m_executionIDIteration;
        private const int CURRENT_SHAKECLIP_INDEX = 0;

        public bool hasClipsLeft => m_queue.Count > 0;
        public NoiseSettings profile => hasClipsLeft ? m_queue[CURRENT_SHAKECLIP_INDEX].Value.info.noiseProfile : null;
        public float amplitude => hasClipsLeft ? m_queue[CURRENT_SHAKECLIP_INDEX].Value.amplitude : 0f;
        public float frequency => hasClipsLeft ? m_queue[CURRENT_SHAKECLIP_INDEX].Value.frequency : 0f;

        public CameraShakeBlendHandle()
        {
            m_queue = new List<Cache<ShakeClip>>();

            m_queueComparison = (x, y) =>
            {
                if (x.Value.priority == y.Value.priority)
                {
                    return x.Value.executionID > y.Value.executionID ? -1 : 1;
                }
                else
                {

                    return x.Value.priority > y.Value.priority ? -1 : 1;
                }
            };

            m_executionIDIteration = 0;
        }

        public void Execute(CameraShakeData data)
        {
            m_executionIDIteration++;
            var cacheClip = Cache<ShakeClip>.Claim();
            cacheClip.Value.Initialize(data, m_executionIDIteration);
            m_queue.Add(cacheClip);
            m_queue.Sort(m_queueComparison);
        }


        public void Update(float deltaTime)
        {
            for (int i = m_queue.Count - 1; i >= 0; i--)
            {
                var cacheClip = m_queue[i];
                if (cacheClip.Value.hasEnded)
                {
                    m_queue.RemoveAt(i);
                    cacheClip.Release();
                }
                else
                {
                    cacheClip.Value.UpdateTimer(deltaTime);
                }
            }
        }
    }
}