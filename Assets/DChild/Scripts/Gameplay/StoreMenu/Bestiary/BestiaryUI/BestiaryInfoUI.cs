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

        private Color m_originalImageColor;
        private Color m_originalSketchColor;

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
            if (m_showDataOf = data)
            {
                m_showDataOf = data;
                UpdateInfo();
            }
        }

        private void UpdateInfo()
        {
            if (m_showDataOf == null)
            {
                creatureNameText = "NOTHING";
                UpdateImage();
                m_location.text = "THE VOID";
                m_description.text = "Dead";
                m_storeNotes.text = "Run, don't look back!!";
                m_hunterNotes.text = "Whatever you do, do not look for this";
            }
            else
            {
                creatureNameText = m_showDataOf.creatureName;
                UpdateImage();
                UpdateLocation(m_showDataOf.locatedIn);
                m_description.text = m_showDataOf.description;
                m_storeNotes.text = m_showDataOf.storeNotes;
                m_hunterNotes.text = m_showDataOf.hunterNotes;
            }
        }

        private void UpdateImage()
        {
            SetImage(m_creatureImage, m_showDataOf.infoImage, m_originalImageColor);
            SetImage(m_sketchImage, m_showDataOf.sketchImage, m_originalSketchColor);
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

        private void SetImage(Image image, Sprite sprite, Color originalColor)
        {
            if (sprite == null)
            {
                image.color = Color.clear;
            }
            else
            {
                image.color = originalColor;
                image.sprite = sprite;
            }
        }

        private void Start()
        {
            m_originalImageColor = m_creatureImage.color;
            m_originalSketchColor = m_sketchImage.color;
        }
    }
}