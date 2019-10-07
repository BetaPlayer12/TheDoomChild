using DChild.Gameplay.Combat;
using Doozy.Engine;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
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
                var ID = eventArgs.target.characterID;
                if (m_progress.HasInfoOf(ID))
                {
                    GameEventMessage.SendEvent("Notification");
                }
                m_progress.SetProgress(ID, true);
            }
        }
    }
}