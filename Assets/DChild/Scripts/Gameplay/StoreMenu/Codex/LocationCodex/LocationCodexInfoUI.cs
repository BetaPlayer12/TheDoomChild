using DChild.Gameplay.Environment;
using DChild.Menu.Codex;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Codex.LocationCodex
{
    public class LocationCodexInfoUI : CodexInfoUI<LocationCodexData>
    {
        [SerializeField]
        private TextMeshProUGUI m_alphabetName;
        [SerializeField]
        private TextMeshProUGUI m_baybayinName;
        [SerializeField]
        private Image m_locationImage;
        [SerializeField]
        private Image m_sketchImage;
        [SerializeField]
        private TextMeshProUGUI m_location;
        [SerializeField]
        private TextMeshProUGUI m_description;



        private string LocationNameText
        {
            set
            {
                m_alphabetName.text = value;
                m_baybayinName.text = value;
            }
        }

        protected override void UpdateInfo()
        {
            if (m_showDataOf == null)
            {
                LocationNameText = "";
                SetImage(m_locationImage, null);
                SetImage(m_sketchImage, null);
                m_location.text = "";
                m_description.text = "";
            }
            else
            {
                LocationNameText = m_showDataOf.name;
                SetImage(m_locationImage, m_showDataOf.indexImage);
                SetImage(m_sketchImage, m_showDataOf.indexImage);
                UpdateLocation(m_showDataOf.locatedIn);
                m_description.text = m_showDataOf.description;

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
            if (sprite == null)
            {
                image.color = Color.clear;
                image.sprite = sprite;
            }
            else
            {
                image.color = Color.white;
                image.sprite = sprite;
            }
        }
    }
}

