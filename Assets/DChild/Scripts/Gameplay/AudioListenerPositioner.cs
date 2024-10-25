using DChild;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    [AddComponentMenu("DChild/Gameplay/Audio/Audio Listener Positioner")]
    public class AudioListenerPositioner : MonoBehaviour
    {
        private enum ToFollow
        {
            Player,
            Camera
        }
        [SerializeField]
        private ToFollow m_toFollowType;

        private Transform m_camera;
        private Transform m_toFollow;
        private static AudioListenerPositioner m_instance;

        private void OnCameraChange(object sender, CameraChangeEventArgs eventArgs)
        {
            if (m_toFollowType == ToFollow.Camera)
            {
                m_camera = eventArgs.camera?.transform ?? null;
                enabled = m_camera;
            }
        }

        public void AttachToCamera()
        {
            m_toFollow = m_camera;
            m_toFollowType = ToFollow.Camera;
        }

        public void AttachToPlayer()
        {
            m_toFollow = GameplaySystem.playerManager.player.character.centerMass;
            m_toFollowType = ToFollow.Player;
        }

        private void Awake()
        {
            if (m_instance)
            {
                Destroy(gameObject);
            }
            else
            {
                m_instance = this;
            }
        }

        private IEnumerator Start()
        {
            GameSystem.CameraChange += OnCameraChange;
            switch (m_toFollowType)
            {
                case ToFollow.Player:
                    while (GameplaySystem.playerManager == null)
                        yield return null;

                    m_toFollow = GameplaySystem.playerManager.player.character.centerMass;
                    break;
                case ToFollow.Camera:
                    m_camera = GameSystem.mainCamera?.transform ?? null;
                    enabled = m_camera;
                    m_toFollow = m_camera;
                    break;
            }
        }

        private void Update()
        {
            if (m_toFollowType == ToFollow.Camera && m_camera == null)
            {
                if (GameSystem.mainCamera != null)
                {
                    m_camera = GameSystem.mainCamera?.transform;
                }
            }
            if(m_toFollow != null)
            {
                var position = m_toFollow.position;
                position.z = 0;
                transform.position = position;
            }
        }

        private void OnDestroy()
        {
            if (this == m_instance)
            {
                m_instance = null;
            }
        }
    }
}
