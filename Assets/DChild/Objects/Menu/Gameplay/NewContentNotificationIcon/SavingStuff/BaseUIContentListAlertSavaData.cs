using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class BaseUIContentListAlertSavaData : UIContentListAlertSaveData
    {
        public BaseUIContentListAlertSavaData[] ContentAlerts;

        public BaseUIContentListAlertSavaData(BaseUIContentListAlertSavaData[] contentAlerts)
        {
            ContentAlerts = contentAlerts;
        }

        public bool HasAlert()
        {
            return true;
        }

        public bool HasAlert(int ContentID)
        {
            return true;
        }

        public UIContentListAlertSaveData GetSubSaveData(int ContentID)
        {
            return ContentAlerts[ContentID];
        }
    }
}

