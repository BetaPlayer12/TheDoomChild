using DChild.Gameplay.Environment;
using DChild.Serialization;
using Holysoft.Collections;
using Holysoft.Event;

namespace DChild.Menu
{
    public struct SelectedCampaignSlotEventArgs : IEventActionArgs
    {
        public SelectedCampaignSlotEventArgs(CampaignSlot slot) : this()
        {
            this.ID = slot.id;
            isNewGame = slot.newGame;
            location = slot.location;
            completion = slot.completion;
            duration = slot.duration;
        }

        public int ID { get; }
        public bool isNewGame { get; }
        public Location location { get; }
        public int completion { get; }
        public TimeKeeper duration { get; }
    }

    public interface ICampaignSelect
    {
        event EventAction<SelectedCampaignSlotEventArgs> CampaignSelected;
    }

    public interface ICampaignSelectEventCaller
    {
        void SendCampaignSelectedEvent();
    }
}