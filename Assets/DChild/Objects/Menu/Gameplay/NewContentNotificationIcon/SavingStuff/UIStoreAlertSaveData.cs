using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class UIStoreAlertSaveData 
    {
        private bool m_mapAlert;
        private BaseUIContentListAlertSavaData m_statAlert;
        private UIContentListAlertSaveData m_itemAlert;
        private UIContentListAlertSaveData m_soulSkillAlert;
        private UIContentListAlertSaveData m_codexAlert;

        public UIStoreAlertSaveData(bool mapAlert, BaseUIContentListAlertSavaData statAlert, UIContentListAlertSaveData itemAlert, UIContentListAlertSaveData soulSkillAlert, UIContentListAlertSaveData codexAlert)
        {
            m_mapAlert = mapAlert;
            m_statAlert = statAlert;
            m_itemAlert = itemAlert;
            m_soulSkillAlert = soulSkillAlert;
            m_codexAlert = codexAlert;
        }

        public bool GetMapAlert()
        {
            return m_mapAlert;
        }

        public UIContentListAlertSaveData GetStatAlert()
        {
            return m_statAlert;
        }

        public UIContentListAlertSaveData GetItemAlert()
        {
            return m_itemAlert;
        }

        public UIContentListAlertSaveData GetSoulSkillAlert()
        {
            return m_soulSkillAlert;
        }

        public UIContentListAlertSaveData GetCodexAlert()
        {
            return m_codexAlert;
        }
    }

}
