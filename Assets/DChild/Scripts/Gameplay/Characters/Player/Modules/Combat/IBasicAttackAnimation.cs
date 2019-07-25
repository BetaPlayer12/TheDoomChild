using Spine.Unity;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IBasicAttackAnimation
    {
        void DoBasicAttack(int comboIndex, HorizontalDirection direction);
        void DoCrouchAttack(int comboIndex, HorizontalDirection direction);
        void DoJumpAttackUpward(int comboIndex, HorizontalDirection direction);
        void DoJumpAttackDownward(int comboIndex, HorizontalDirection direction);
        void DoJumpAttackForward(int comboIndex, HorizontalDirection direction);
        void DoUpwardAttack(int comboIndex, HorizontalDirection direction);
        SkeletonAnimation skeletonAnimation { get; }
        string currentAttackAnimation { get; }
    }
}