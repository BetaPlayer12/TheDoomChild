namespace DChild.Gameplay.Characters.Enemies
{
    public class HauntingGhostAnimation : CombatCharacterAnimation
    {
        public enum MoveDirection
        {
            Forward,
            Backward
        }

        public const string ANIMATION_DESPAWN = "Despawn";
        public const string ANIMATION_DEATH = "Death";
        public const string ANIMATION_SPAWN = "Spawn";
        public const string ANIMATION_ATTACK_ANTICIPATION = "Attack_Aniticipation";
        public const string ANIMATION_ATTACK_LOOP = "Attack_Loop";
        public const string ANIMATION_ATTACK_DESPAWN = "Despawn_Afterattack";

        public override void DoDeath() => SetAnimation(0, "Death", false);
        public override void DoIdle() => SetAnimation(0, "Idle", true);
        public void DoMove(MoveDirection direction) => SetAnimation(0, $"Move_{direction}", true);
        public void DoDespawn() => SetAnimation(0, ANIMATION_DESPAWN, false);
        public void DoSpawn() => SetAnimation(0, ANIMATION_SPAWN, false);
        public void DoAttack()
        {
            SetAnimation(0, ANIMATION_ATTACK_ANTICIPATION, false);
            AddAnimation(0, ANIMATION_ATTACK_LOOP, true, 0f);
        }

        public void DoAttackDespawn()
        {
           var track = SetAnimation(0, ANIMATION_ATTACK_DESPAWN, false);
            track.MixDuration = 0f;
        }
    }

}