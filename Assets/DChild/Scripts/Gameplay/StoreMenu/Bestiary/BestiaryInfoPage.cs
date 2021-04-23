using DChild.Gameplay.Environment;
using Holysoft.UI;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Bestiary
{
    public class BestiaryInfoPage : MonoBehaviour
    {
        [ShowInInspector, OnValueChanged("UpdateInfo")]
        private BestiaryData m_showDataOf;

        [SerializeField]
        private TextMeshProUGUI m_name;
        [SerializeField]
        private Image m_image;
        [SerializeField]
        private TextMeshProUGUI m_location;
        [SerializeField]
        private TextMeshProUGUI m_description;
        [SerializeField]
        private Image m_sketchImage;

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
                m_name.text = "NOTHING";
                m_image.sprite = null;
                m_location.text = "THE VOID";
                m_description.text = "Dead";
                m_sketchImage.sprite = null;
            }
            else
            {
                m_name.text = m_showDataOf.creatureName;
                m_image.sprite = m_showDataOf.infoImage;
                UpdateLocation(m_showDataOf.locatedIn);
                m_description.text = m_showDataOf.description;
                m_sketchImage.sprite = m_showDataOf.sketchImage;
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
    }
}