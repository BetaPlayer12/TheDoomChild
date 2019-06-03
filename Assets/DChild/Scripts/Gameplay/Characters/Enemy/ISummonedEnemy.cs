using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public interface ISummonedEnemy
    {
        void SummonAt(Vector2 position);
        void DestroyObject();
    }
}
