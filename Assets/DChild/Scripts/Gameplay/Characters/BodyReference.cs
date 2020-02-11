using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

namespace DChild.Gameplay
{
    [System.Serializable]
    public class BodyReference
    {
        public enum BodyPart
        {
            OverHead,
            Head,
            Body,
            Feet,
            [HideInInspector]
            _COUNT,
        }

        [System.Serializable]
        private class Info
        {
            [SerializeField]
            private BodyPart m_label;
            [SerializeField]
            private Transform m_bodyPart;

            public Info(BodyPart m_label)
            {
                this.m_label = m_label;
                m_bodyPart = null;
            }

            public BodyPart label => m_label;
            public Transform bodyPart => m_bodyPart;
        }

        [SerializeField, InfoBox("Info The requested body part does not exist then this is returned")]
        private Transform m_default;
        [SerializeField, ListDrawerSettings(HideAddButton = true, OnTitleBarGUI = "OnTitleBarGUI"),
        ValidateInput("ValidateInput", IncludeChildren = true, MessageType = InfoMessageType.Error, DefaultMessage = "Info should have unique labels")]
        private Info[] m_list;

        public Transform GetBodyPart(BodyPart bodyPart)
        {
            for (int i = 0; i < m_list.Length; i++)
            {
                if (m_list[i].label == bodyPart)
                {
                    return m_list[i].bodyPart;
                }
            }

            return m_default;
        }

#if UNITY_EDITOR
        private void OnTitleBarGUI()
        {
            if (m_list.Length < (int)BodyPart._COUNT)
            {
                if (SirenixEditorGUI.IconButton(EditorIcons.Plus))
                {
                    BodyPart unUsedBodyPart = BodyPart._COUNT;
                    for (int i = 0; i < (int)BodyPart._COUNT; i++)
                    {
                        if (ContainsBodyPart(m_list, (BodyPart)i) == false)
                        {
                            unUsedBodyPart = (BodyPart)i;
                            break;
                        }
                    }

                    var newList = new Info[m_list.Length + 1];
                    for (int i = 0; i < m_list.Length; i++)
                    {
                        newList[i] = m_list[i];
                    }
                    m_list = newList;
                    m_list[m_list.Length - 1] = new Info(unUsedBodyPart);
                }
            }
        }

        private bool ValidateInput(Info[] list)
        {
            List<BodyPart> m_usedLabel = new List<BodyPart>();
            for (int i = 0; i < list.Length; i++)
            {
                if (m_usedLabel.Contains(list[i].label))
                {
                    return false;
                }
                else
                {
                    m_usedLabel.Add(list[i].label);
                }
            }
            return true;
        }

        private bool ContainsBodyPart(Info[] list, BodyPart part)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].label == part)
                {
                    return true;
                }
            }
            return false;
        }
#endif
    }
}