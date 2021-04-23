using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems.Journal
{
    public class JournalNotifier : MonoBehaviour
    {
        [SerializeField]
        private JournalData m_data;

        private static Player m_player;
        private static JournalProgress m_journalProgess;

        [Button, HideInEditorMode]
        public void SendNotification()
        {
            m_journalProgess.UpdateJournal(m_data);
        }

        private void OnEnable()
        {
            var currentPlayer = GameplaySystem.playerManager.player;
            if (m_player == null || m_player == currentPlayer)
            {
                m_player = GameplaySystem.playerManager.player;
                m_journalProgess = m_player.GetComponentInChildren<JournalProgress>();
            }
        }
    }
}
