using DChild.Gameplay.Characters.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public interface ISummonedEnemy
    {
        //Debug Fix
        void SummonAt(Vector2 position, AITargetInfo target);
        void DestroyObject();
    }
}
