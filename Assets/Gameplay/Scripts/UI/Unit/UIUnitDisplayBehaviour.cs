using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonCrashers
{
    public class UIUnitDisplayBehaviour : MonoBehaviour
    {

        [Header("Unit Controller")]
        public UnitController unit;

        [Header("Unit Display Info")]
        public UITextBehaviour textDisplayUnitName;
        public UIImageBehaviour imageDisplayUnitAvatar;

        void Awake()
        {
            SetupUIDisplay();
        }

        void SetupUIDisplay()
        {
            SetupUnitName(unit.data.unitName);
            SetupUnitAvatar(unit.data.unitAvatar);
        }

        public void SetupUnitName(string newUnitName)
        {
            if(textDisplayUnitName != null)
            {
                textDisplayUnitName.SetText(newUnitName);
            }
        }

        public void SetupUnitAvatar(Sprite newUnitAvatar)
        {
            if(imageDisplayUnitAvatar != null)
            {
                imageDisplayUnitAvatar.SetImage(newUnitAvatar);
            }
        }
    }
}