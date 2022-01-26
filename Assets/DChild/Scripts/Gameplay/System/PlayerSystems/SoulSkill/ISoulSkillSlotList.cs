using Holysoft.Event;

namespace DChild.Gameplay.SoulSkills
{
    public interface ISoulSkillSlotList
    {
        event EventAction<EventActionArgs> ChangeInContent;

        int currentMaxCapacity { get; }
        int appliedSoulSkillCount { get; }

        ISoulSkill GetSoulSkill(int index);
    }
}
