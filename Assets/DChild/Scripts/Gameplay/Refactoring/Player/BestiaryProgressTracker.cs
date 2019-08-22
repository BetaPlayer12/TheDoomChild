using DChild.Gameplay.Combat;
using Refactor.DChild.Gameplay.Combat;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Players
{
    public class BestiaryProgressTracker : MonoBehaviour
    {
        [SerializeField]
        private BestiaryProgress m_progress;
        [SerializeField]
        private Attacker m_attacker;

        private void Awake()
        {
            m_attacker.TargetDamaged += OnTargetDamaged;
        }

        private void OnTargetDamaged(object sender, CombatConclusionEventArgs eventArgs)
        {
            if (eventArgs.target.instance.isAlive == false && eventArgs.target.hasID)
            {
                m_progress.SetProgress(eventArgs.target.characterID, true);
            }
        }
    }
}