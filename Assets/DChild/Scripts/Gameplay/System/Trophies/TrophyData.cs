using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Trohpies
{

    [CreateAssetMenu(fileName = "TrophyData", menuName = "DChild/Database/Trophy Data")]
    public class TrophyData : DatabaseAsset
    {
        ////Uncommnet this one once connected to steam
        //[SerializeField]
        //private string m_steamID;
        [SerializeField, ToggleGroup("m_enableEdit")]
        private bool m_enableEdit;
        [SerializeField, ToggleGroup("m_enableEdit"), PreviewField(100), HorizontalGroup("m_enableEdit/Split")]
        private Sprite m_icon;
        [SerializeField, ToggleGroup("m_enableEdit"), PreviewField(100), HorizontalGroup("m_enableEdit/Split")]
        private Sprite m_lockedIcon;
        [SerializeField, ToggleGroup("m_enableEdit")]
        private string m_description;
        [OdinSerialize, ToggleGroup("m_enableEdit")]
        private ITrophyModule[] m_modules = new ITrophyModule[0];

#if UNITY_EDITOR
        protected override IEnumerable GetIDs()
        {
            throw new System.NotImplementedException();
        }

        protected override void UpdateReference()
        {
            throw new System.NotImplementedException();
        }
#endif
        public TrophyInfo CreateInfo() => new TrophyInfo(m_ID, m_name, m_icon, m_lockedIcon, m_description);

        public TrophyHandle CreateHandle()
        {
            var moduleInstances = new ITrophyModule[m_modules.Length];
            for (int i = 0; i < moduleInstances.Length; i++)
            {
                moduleInstances[i] = m_modules[i].CreateCopy();
            }
            return new TrophyHandle(m_ID, moduleInstances);
        }
    }
}