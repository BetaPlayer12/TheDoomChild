using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelTurn : MonoBehaviour
{
    [System.Serializable]
    private class AttachmentHandle
    {
#if UNITY_EDITOR
        [SerializeField]
        private SkeletonDataAsset m_asset;
#endif
        [SerializeField, SpineSlot(dataField = "m_asset")]
        private string m_slot;

        [SerializeField, SpineAttachment(dataField = "m_asset")]
        private string m_leftVersion;
        [SerializeField, SpineAttachment(dataField = "m_asset")]
        private string m_rightVersion;

        public void SetAttachment(Skeleton skeleton, HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                skeleton.SetAttachment(m_slot, m_leftVersion);
            }
            else
            {
                skeleton.SetAttachment(m_slot, m_rightVersion);
            }
        }
    }

    [SerializeField]
    private HorizontalDirection m_initialFacingDirection;
    [SerializeField]
    private bool m_executeOnTurn;
    [SerializeField, Spine.Unity.SpineAnimation]
    private string m_turnAnimation;
    [SerializeField, SpineEvent]
    private string m_turnEvent;
    [SerializeField]
    private AttachmentHandle m_rightShoulder;
    [SerializeField]
    private AttachmentHandle m_leftShoulder;

    private Skeleton m_skeleton;
    private Character m_character;

    // Start is called before the first frame update
    void Start()
    {
        var animation = GetComponent<SkeletonAnimation>();
        animation.state.Event += OnEvent;
        animation.state.Interrupt += OnComplete;
        m_skeleton = animation.skeleton;

        m_character = GetComponentInParent<Character>();
        UpdateAttachments();
        if (m_executeOnTurn)
        {
            m_character.CharacterTurn += OnCharacterTurn;
        }
    }

    private void OnCharacterTurn(object sender, FacingEventArgs eventArgs)
    {
        UpdateAttachments();
    }

    private void OnComplete(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == m_turnAnimation)
        {
            m_skeleton.ScaleX = m_character.facing == m_initialFacingDirection ? 1 : -1;
        }
    }

    private void OnEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == m_turnEvent)
        {
            UpdateAttachments();
        }
    }

    private void UpdateAttachments()
    {
        m_rightShoulder.SetAttachment(m_skeleton, m_character.facing);
        m_leftShoulder.SetAttachment(m_skeleton, m_character.facing);
    }
}
