using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public interface IEyeBossAttacks
    {
        IEnumerator ExecuteAttack();
        IEnumerator ExecuteAttack(Vector2 PlayerPosition);
    }
}

//Note: This is for The One Attacks because it has many complicated attacks

