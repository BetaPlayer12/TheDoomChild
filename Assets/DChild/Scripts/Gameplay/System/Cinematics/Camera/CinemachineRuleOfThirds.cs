using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    [ExecuteInEditMode]
    [SaveDuringPlay]
    [AddComponentMenu("")] // Hide in menu
    public class CinemachineRuleOfThirds : CinemachineExtension
    {
        [SerializeField]
        private Vector2 m_offset;

        private bool m_isConnected;
        private Character m_character;

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (m_character != null)
            {
                var offset = m_offset * (int)m_character.facing;
                var position = vcam.transform.position;
                position.x += offset.x;
                position.y += offset.y;
                vcam.transform.position = position;
            }
        }

        protected override void ConnectToVcam(bool connect)
        {
            base.ConnectToVcam(connect);
            if (m_isConnected != connect)
            {
                m_isConnected = connect;
                if (m_isConnected)
                {
                    m_character = GameplaySystem.playerManager.player.character;
                }
            }
        }
    }

}