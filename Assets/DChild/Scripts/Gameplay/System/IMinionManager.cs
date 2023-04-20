using DChild.Gameplay.Characters.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinionManager
{
    void Register(ICombatAIBrain minion);

    void Unregister(ICombatAIBrain minion);

    void ForbidAllFromAttackingTarget(bool willAttackTarget);
    void IgnoreAllTargets(bool willIgnoreTarget);
    void IgnoreCurrentTarget();

    void ForcePassiveIdle(bool willForcePassive);
}
