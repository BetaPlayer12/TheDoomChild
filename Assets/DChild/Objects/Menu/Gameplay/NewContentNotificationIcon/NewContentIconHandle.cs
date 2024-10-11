using DChild.Gameplay.Systems;
using UnityEngine;

namespace DChild.UI
{
    public abstract class NewContentIconHandle : MonoBehaviour
    {
        public abstract bool hasAlert { get; protected set; }

        public virtual void AlertSeen()
        {
            hasAlert = false;

            HideAlert();

        }
        public virtual  void AlertNotSeen()
        {
            hasAlert = true;

            ShowAlert();
        }

        public abstract void ShowAlert();

        public abstract void HideAlert();

        public abstract UIContentListAlertSaveData SaveData();
        public abstract void LoadData(UIContentListAlertSaveData data);
    }
}
