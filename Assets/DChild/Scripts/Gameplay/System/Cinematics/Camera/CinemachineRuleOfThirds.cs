using Cinemachine;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
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
        [SerializeField, OnValueChanged("OnOffsetChanged")]
        private Vector2 m_offset;
        [SerializeField, MinValue(0.1f)]
        private float m_interpolationSpeed = 2;

        private Vector2 m_destinationOffset;
        private Vector2 m_startOffset;
        private Vector2 m_currentOffset;
        private float m_lerpValue;

        private bool m_isConnected;
        private Character m_character;
        private CinemachineBrain m_brain;

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (Application.isPlaying == false)
            {
                m_character = VirtualCamera.Follow.GetComponentInParent<Character>();
            }

            if (m_character != null)
            {
                if (stage == CinemachineCore.Stage.Body)
                {
                    if (Application.isPlaying)
                    {
                        if (m_lerpValue < 1)
                        {
                            m_lerpValue += m_interpolationSpeed * Time.unscaledDeltaTime;
                            m_currentOffset = Vector2.Lerp(m_startOffset, m_destinationOffset, m_lerpValue);
                        }
                    }
                    else
                    {
                        m_currentOffset = m_offset * (int)m_character.facing;
                    }

                    var position = state.PositionCorrection;
                    position.x += m_currentOffset.x;
                    position.y += m_currentOffset.y;
                    state.PositionCorrection = position;
                }
            }
        }
        private void OnTurn(object sender, FacingEventArgs eventArgs)
        {
            m_destinationOffset = m_offset * (int)eventArgs.currentFacingDirection;
            m_startOffset = -m_destinationOffset;
            m_lerpValue = 1 - Mathf.Abs((m_currentOffset - m_destinationOffset).magnitude / (m_startOffset - m_destinationOffset).magnitude);
        }

        private void OnCameraActivated(ICinemachineCamera arg0, ICinemachineCamera arg1)
        {
            if (VirtualCamera == arg0)
            {
                m_isConnected = true;
                m_character = GameplaySystem.playerManager?.player?.character ?? null;
                if (m_character)
                {
                    m_destinationOffset = m_offset * (int)m_character.facing;
                    m_startOffset = m_destinationOffset;
                    m_currentOffset = m_startOffset;
                    m_character.CharacterTurn += OnTurn;
                    m_lerpValue = 1;
                }
            }
            else
            {
                m_isConnected = false;
            }
        }


        protected override void Awake()
        {
            base.Awake();
            m_brain = FindObjectOfType<CinemachineBrain>();
            m_brain.m_CameraActivatedEvent.AddListener(OnCameraActivated);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_brain.m_CameraActivatedEvent.RemoveListener(OnCameraActivated);
            m_brain = null;
        }

#if UNITY_EDITOR
        private void OnOffsetChanged()
        {
            m_destinationOffset = m_offset * (int)m_character.facing;
            m_startOffset = m_destinationOffset;
            m_currentOffset = m_startOffset;
            m_lerpValue = 1;
        }
#endif

    }

}