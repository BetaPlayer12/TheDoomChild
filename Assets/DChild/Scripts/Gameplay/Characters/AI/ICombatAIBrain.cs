﻿using DChild.Gameplay;
using DChild.Gameplay.Combat;
#if UNITY_EDITOR
using System;
using UnityEngine;
#endif

namespace DChild.Gameplay.Characters.AI
{
    public interface ICombatAIBrain
    {
        void SetTarget(IDamageable damageable, Character m_target = null);

#if UNITY_EDITOR
        Type aiDataType { get; }
        void SetData(AIData data);
        void InitializeField(Character character, SpineRootAnimation spineRoot, Damageable damageable, Transform centerMass);
#endif
    }
}