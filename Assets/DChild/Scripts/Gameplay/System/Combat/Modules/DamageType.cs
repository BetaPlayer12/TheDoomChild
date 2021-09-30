using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [Flags]
    public enum DamageType
    {
        Physical = 1 << 0,
        Fire = 1 << 1,
        Lightning = 1 << 2,
        Ice = 1 << 3,
        Holy = 1 << 4,
        Dark = 1 << 5,
        Acid = 1 << 6,
        Poison = 1 << 7,
        True = 1 << 8,
        [HideInInspector]
        All = Physical | Fire | Lightning | Ice | Holy | Dark | Acid | Poison | True,
    }
}