using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class CompositeUIContentListAlertSaveData : UIContentListAlertSaveData
    {
        public UIContentListAlertSaveData[] ContentAlerts;

        public CompositeUIContentListAlertSaveData(UIContentListAlertSaveData[] contentAlerts)
        {
            ContentAlerts = contentAlerts;
        }

        public bool HasAlert()
        {
            return true;
        }

        public UIContentListAlertSaveData GetSubSaveData(int ContentID)
        {
            return ContentAlerts[ContentID];
        }
    }
}
