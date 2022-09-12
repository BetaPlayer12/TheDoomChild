using DChild.Gameplay;
using DChild.Gameplay.Combat;
#if UNITY_EDITOR
using System;
using UnityEngine;
#endif

namespace DChild.Gameplay.Characters.AI
{
    public interface ICombatAIBrain
    {
        bool enabled { get; set; }
        void SetTarget(IDamageable damageable, Character m_target = null);

        void ForbidFromAttackTarget(bool value);
        void IgnoreCurrentTarget();
        void IgnoreAllTargets(bool value);
        void ReturnToSpawnPoint();

#if UNITY_EDITOR

        CharacterStatsData statsData { get; set; }
        Type aiDataType { get; }
        void SetData(AIData data);
        void InitializeField(Character character, SpineRootAnimation spineRoot, Damageable damageable, Transform centerMass);
#endif
    }
}