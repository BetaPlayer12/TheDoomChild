using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.Items;
using DChild.Gameplay.Systems;
using DChild.Gameplay.Systems.Journal;
using DChild.Gameplay.Systems.Lore;
using PixelCrushers.DialogueSystem;

namespace DChild.Gameplay.UI
{
    public interface IUINotificationManager
    {
        void ShowJournalUpdateNotification(JournalData journalData);

        void QueueNotification(PrimarySkill skill);

        void QueueNotification(SoulSkill soulSkill);

        void QueueNotification(LoreData data);

        void QueueNotification(ItemData itemData);

        void QueueNotification(QuestEntryArgs questInfo);

        void QueueNotification(LootList lootList);

        void QueueNotification(StoreNotificationType notificationType);

        void RemoveAllQueuedNotifications();
    }
}