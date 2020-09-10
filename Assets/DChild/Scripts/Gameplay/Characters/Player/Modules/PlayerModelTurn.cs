using Spine;
using Spine.Unity;
using UnityEngine;
namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerModelTurn : MonoBehaviour, IComplexCharacterModule
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
        private AttachmentHandle m_rightShoulder;
        [SerializeField]
        private AttachmentHandle m_leftShoulder;

        private Skeleton m_skeleton;
        private Character m_character;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            m_character.CharacterTurn += OnCharacterTurn;
        }

        private void OnCharacterTurn(object sender, FacingEventArgs eventArgs)
        {
            UpdateAttachments();
        }

        private void UpdateAttachments()
        {
            var facing = m_character.facing;
            m_rightShoulder.SetAttachment(m_skeleton, facing);
            m_leftShoulder.SetAttachment(m_skeleton, facing);
        }

        private void Start()
        {
            m_skeleton = m_character.GetComponentInChildren<SkeletonAnimation>().Skeleton;
            UpdateAttachments();
        }


    } 
}
