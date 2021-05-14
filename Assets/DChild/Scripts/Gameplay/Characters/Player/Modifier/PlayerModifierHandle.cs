using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [AddComponentMenu("DChild/Gameplay/Player/Player Modifier Handle")]
    public class PlayerModifierHandle : MonoBehaviour, IPlayerModifer
    {  
        [ShowInInspector]
        private Dictionary<PlayerModifier, float> m_modifiers;

        public event EventAction<ModifierChangeEventArgs> ModifierChange;

        public void Add(PlayerModifier modifier, float value)
        {
            if (m_modifiers.ContainsKey(modifier))
            {
                m_modifiers[modifier] += value;
            }
            else
            {
                m_modifiers.Add(modifier, value);
            }
            ModifierChange?.Invoke(this, new ModifierChangeEventArgs(modifier, m_modifiers[modifier]));
        }

        public void Set(PlayerModifier modifier, float value)
        {
            if (m_modifiers.ContainsKey(modifier))
            {
                m_modifiers[modifier] = value;
            }
            else
            {
                m_modifiers.Add(modifier, value);
            }
            ModifierChange?.Invoke(this, new ModifierChangeEventArgs(modifier, value));
        }

        public float Get(PlayerModifier modifier) => m_modifiers[modifier];

        private void Awake()
        {
            m_modifiers = new Dictionary<PlayerModifier, float>();
            var size = (int)PlayerModifier._COUNT;
            for (int i = 0; i < size; i++)
            {
                var key = (PlayerModifier)i;
                if (m_modifiers.ContainsKey(key) == false)
                {
                    m_modifiers.Add(key, 1);
                }
            }
        }
    }
}