namespace DChild.Gameplay.Systems.Journal
{
    public class SoulEssenceJournalNotifier : JournalNotifier
    {
        private bool m_notified = false;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (m_notified == false)
            {
                m_player.inventory.OnAmountAdded += OnNewSoulEssence;
            }
        }

        private void OnNewSoulEssence(object sender, CurrencyUpdateEventArgs eventArgs)
        {
            if (m_notified == false)
            {
                SendNotification();
                m_notified = true;
                m_player.inventory.OnAmountAdded -= OnNewSoulEssence;
            }
        }
    }
}