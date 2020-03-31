using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    public interface IPhaseConditionTemplate<T> where T : System.Enum
    {
        IPhaseConditionHandle<T> CreateHandle(Character character);
    }

   
}