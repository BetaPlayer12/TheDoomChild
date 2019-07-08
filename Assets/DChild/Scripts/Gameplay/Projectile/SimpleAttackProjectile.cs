namespace DChild.Gameplay.Projectiles
{
    public class SimpleAttackProjectile : AttackProjectile
    {
        protected override void Collide()
        {
            UnloadProjectile();
        }
    }
}
