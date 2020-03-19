namespace DChild.Gameplay.Combat
{
    public struct DamageInfo
    {
        public AttackDamage damage { get; set; }
        public bool isMagicalDamage { get; }
        public bool isHeal { get; set; }

        public DamageInfo(AttackDamage attackDamage)
        {
            damage = attackDamage;
            isMagicalDamage = AttackDamage.IsMagicAttack(damage.type);
            isHeal = false;
            //Content
        }
    }
}