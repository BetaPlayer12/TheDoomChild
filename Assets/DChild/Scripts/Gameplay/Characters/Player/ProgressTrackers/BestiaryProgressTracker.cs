using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems;
using DChild.Menu.Bestiary;
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


        public void RecordCreatureToBestiary(int ID)
        {
            if (m_progress.HasInfoOf(ID) == false)
            {
                GameplaySystem.gamplayUIHandle.PromptBestiaryNotification();
            }
            m_progress.SetProgress(ID, true);
        }

        public void RecordCreatureToBestiary(BestiaryData data)
        {
            RecordCreatureToBestiary(data.id);
        }

        private void Awake()
        {
            m_attacker.TargetDamaged += OnTargetDamaged;
        }

        private void OnTargetDamaged(object sender, CombatConclusionEventArgs eventArgs)
        {
            if (eventArgs.target.instance.isAlive == false && eventArgs.target.hasBestiaryData)
            {
                RecordCreatureToBestiary(eventArgs.target.bestiaryID);
            }
        }

#if UNITY_EDITOR
        public void Initialize(GameObject character)
        {
            m_attacker = character.GetComponent<Attacker>();
        }
#endif

    }
}