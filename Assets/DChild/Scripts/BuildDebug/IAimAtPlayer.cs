using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;

namespace DChild.Gameplay.Characters.AI
{
    public interface IAimAtPlayer
    {
        void SetPlayerPosition(Vector2 playerPos);
    }
}

