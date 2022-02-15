using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Configurations
{
    [System.Serializable]
    public class ColliderColor
    {
        [SerializeField, LabelText("Color Group"), ValueDropdown("GetOptions"), HorizontalGroup]
        private string m_colorName;
        [SerializeField, LabelText("Color Group"), ShowIf("isCustomColor")]
        private Color m_color;

        private const string CUSTOMOPTION = "Custom";
        private bool isCustomColor => m_colorName == CUSTOMOPTION;
        public Color color
        {
            get
            {
                return isCustomColor ? m_color : ColoredColliderSettings.instance.GetColor(m_colorName);
            }
        }

        private IEnumerable GetOptions()
        {
            ValueDropdownList<string> list = new ValueDropdownList<string>();
            var options = ColoredColliderSettings.instance.GetOptions();
            for (int i = 0; i < options.Length; i++)
            {
                list.Add(options[i]);
            }
            list.Add(CUSTOMOPTION);
            return list;
        }
    }
}