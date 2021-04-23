using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    public class CombatAIManager : SerializedMonoBehaviour
    {
        [SerializeField, ValueDropdown("GetCombatAIs", IsUniqueList = true)]
        private List<ICombatAIBrain> m_combatAI = new List<ICombatAIBrain>();
        private bool m_allArePassive;

        public void Add(ICombatAIBrain instance)
        {
            if (m_combatAI.Contains(instance) == false)
            {
                m_combatAI.Add(instance);
                instance.BecomePassive(m_allArePassive);
            }
        }

        public void Remove(ICombatAIBrain instance)
        {
            if (m_combatAI.Contains(instance))
            {
                m_combatAI.Remove(instance);
                instance.BecomePassive(false);
            }
        }

        public void MakeAllPassive(bool value)
        {
            m_allArePassive = value;
            for (int i = 0; i < m_combatAI.Count; i++)
            {
                if (m_combatAI[i] != null)
                {
                    m_combatAI[i].BecomePassive(m_allArePassive);
                }
            }
        }

#if UNITY_EDITOR
        private IEnumerable GetCombatAIs()
        {
            Func<Transform, string> getPath = null;
            getPath = x => (x ? getPath(x.parent) + "/" + x.gameObject.name : "");
            var brains = GameObject.FindObjectsOfType<Damageable>().Select(x => x.GetComponent<ICombatAIBrain>()).ToArray();
            ValueDropdownList<ICombatAIBrain> list = new ValueDropdownList<ICombatAIBrain>();
            for (int i = 0; i < brains.Length; i++)
            {
                if (brains[i] != null)
                {
                    list.Add(new ValueDropdownItem<ICombatAIBrain>(getPath(((MonoBehaviour)brains[i]).transform), brains[i]));
                }
            }
            return list;
        }
#endif
    }
}

