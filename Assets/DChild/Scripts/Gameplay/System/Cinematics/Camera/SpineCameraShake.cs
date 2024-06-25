using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using System;
using UnityEngine;

namespace DChild.Gameplay.Cinematics.Cameras
{
    public class SpineCameraShake : MonoBehaviour
    {
        [System.Serializable]
        public class Configuration
        {
            [SerializeField, HideInInspector]
            private SkeletonDataAsset m_skeletonData;

            [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData")]
            private string m_animation;
            [SerializeField]
            private CameraShakeData m_camShakeData;

            public string animation => m_animation;
            public CameraShakeData camShakeData => m_camShakeData;

            public void Initialize(SkeletonDataAsset skeletonData)
            {
                m_skeletonData = skeletonData;
            }
        }

        [SerializeField]
        private SkeletonAnimation m_skeleton;

        [SerializeField]
        private Configuration[] m_config;


        [Button]
        private void UpdateConfigurations()
        {
            for (int i = 0; i < m_config.Length; i++)
            {
                m_config[i].Initialize(m_skeleton.skeletonDataAsset);
            }
        }

        private void OnAnimationStart(TrackEntry trackEntry)
        {
            var animation = trackEntry.Animation.Name;
            for (int i = 0; i < m_config.Length; i++)
            {
                if (m_config[i].animation == animation)
                {
                    //GameplaySystem.cinema.ExecuteCameraShake(m_config[i].camShakeData);
                }
            }
        }

        private void Start()
        {
            m_skeleton.AnimationState.Start += OnAnimationStart;
        }

    }
}