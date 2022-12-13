using DChild.Gameplay.Characters.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public interface IEyeBossAttacks
    {
        IEnumerator ExecuteAttack();
        IEnumerator ExecuteAttack(Vector2 PlayerPosition);
        IEnumerator ExecuteAttack(AITargetInfo Target);
    }
}

//Note: This is for The One Attacks because it has many complicated attacks

