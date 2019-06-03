using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public abstract class StatUI : MonoBehaviour
    {
        public abstract float currentValue { set; }
        public abstract float maxValue { set; }
    }
}