using DChild.Gameplay;
using DChild.Gameplay.UI;
using DChild.Menu.Codex;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Codex.Lore
{
    public class LoreCodexProgressTracker : CodexProgressTracker<LoreCodexList, LoreCodexData>
    {

        public void RecordLoreToCodex(int ID)
        {
            if (HasInfoOf(ID) == false)
            {
                GameplaySystem.gamplayUIHandle.notificationManager.QueueNotification(StoreNotificationType.Bestiary);
            }
            SetProgress(ID, true);
        }

        public void RecordCharacterToCodex(LoreCodexData data)
        {
            RecordLoreToCodex(data.id);
        }

        private void Awake()
        {

        }

#if UNITY_EDITOR
        public void Initialize(GameObject character)
        {

        }
#endif
    }
}

