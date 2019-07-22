using DChild.Gameplay.Characters.Players;
using DChild.Serialization;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Players
{

    public class PlayerSkills : SerializedMonoBehaviour, IPrimarySkills
    {
        [SerializeField, HideReferenceObjectPicker]
        private Dictionary<PrimarySkill, bool> m_skills = new Dictionary<PrimarySkill, bool>();

        public bool IsEnabled(PrimarySkill skill) => m_skills[skill];

        public void Enable(PrimarySkill skill, bool enableSkill) => m_skills[skill] = enableSkill;
    }
}