using Holysoft.UI;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Bestiary
{
    public class BestiaryInfoPage : SimpleUICanvas
    {
        private BestiaryData m_showDataOf;
        [SerializeField]
        private TextMeshProUGUI m_name;
        [SerializeField]
        private SkeletonAnimation m_skeleton;
        [SerializeField]
        private TextMeshProUGUI m_description;
        [SerializeField]
        private Image m_sketchImage;

        public void ShowInfo(BestiaryData data)
        {
            m_showDataOf = data;
            m_name.text = data.creatureName.ToUpper();
            if (m_skeleton != null)
            {
                data.SetupSpine(m_skeleton);
            }
            m_description.text = data.description;
            m_sketchImage.sprite = data.sketchImage;
        }
    }
}