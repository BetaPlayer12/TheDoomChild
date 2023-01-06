using DChild.Gameplay.Combat;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Narrative
{
    public class DamageableVariableTracker : DialogueVariableTracker
    {
        [SerializeField,]
        private Damageable[] m_toTrack;

        private void Awake()
        {
            for (int i = 0; i < m_toTrack.Length; i++)
            {
                m_toTrack[i].Destroyed += OnDamageableDestroyed;
            }
        }

        private void OnDamageableDestroyed(object sender, EventActionArgs eventArgs)
        {
            RunLua();
        }
    }
}