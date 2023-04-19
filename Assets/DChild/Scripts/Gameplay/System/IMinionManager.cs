using DChild.Gameplay.Characters.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinionManager
{
    void Register(ICombatAIBrain minion);

    void Unregister(ICombatAIBrain minion);
}
