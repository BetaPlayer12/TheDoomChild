using DChild.Gameplay.Environment;
using Holysoft.UI;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Bestiary
{
    public class BestiaryInfoUI : MonoBehaviour
    {
        [ShowInInspector, OnValueChanged("UpdateInfo")]
        private BestiaryData m_showDataOf;

        [SerializeField]
        private TextMeshProUGUI m_alphabetName;
        [SerializeField]
        private TextMeshProUGUI m_baybayinName;
        [SerializeField]
        private Image m_creatureImage;
        [SerializeField]
        private Image m_sketchImage;
        [SerializeField]
        private TextMeshProUGUI m_location;
        [SerializeField]
        private TextMeshProUGUI m_description;

        [SerializeField]
        private TextMeshProUGUI m_storeNotes;
        [SerializeField]
        private TextMeshProUGUI m_hunterNotes;

        private string creatureNameText
        {
            set
            {
                m_alphabetName.text = value;
                m_baybayinName.text = value;
            }
        }

        public void ShowInfo(BestiaryData data)
        {
            if (m_showDataOf != data)
            {
                m_showDataOf = data;
                UpdateInfo();
            }
            else if(data == null)
            {
                UpdateInfo();
            }
        }

        private void UpdateInfo()
        {
            if (m_showDataOf == null)
            {
                creatureNameText = "";
                SetImage(m_creatureImage, null);
                SetImage(m_sketchImage, null);
                m_location.text = "";
                m_description.text = "";
                m_storeNotes.text = "";
                m_hunterNotes.text = "";
            }
            else
            {
                creatureNameText = m_showDataOf.creatureName;
                SetImage(m_creatureImage, m_showDataOf.infoImage);
                SetImage(m_sketchImage, m_showDataOf.sketchImage);
                UpdateLocation(m_showDataOf.locatedIn);
                m_description.text = m_showDataOf.description;
                m_storeNotes.text = m_showDataOf.storeNotes;
                m_hunterNotes.text = m_showDataOf.hunterNotes;
            }
        }

        private void UpdateLocation(IReadOnlyList<Location> locations)
        {
            m_location.text = "";
            for (int i = 0; i < locations.Count; i++)
            {
                m_location.text += locations[i].ToString().Replace('_', ' ');
                if (i < locations.Count - 1)
                {
                    m_location.text += "/";
                }
            }
        }

        private void SetImage(Image image, Sprite sprite)
        {
            image.sprite = sprite;
            if (sprite == null)
            {
                image.color = Color.clear;
            }
            else
            {
                image.color = Color.white;
            }
        }
    }
}