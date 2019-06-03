namespace DChild.Gameplay.Combat
{
    public struct DamageInfo
    {
        public int damage { get; set; }
        public AttackType damageType { get; }
        public bool isMagicalDamage { get; }
        public bool isCrit { get; set; }
        public bool isHeal { get; set; }

        public DamageInfo(AttackDamage attackDamage)
        {
            damage = attackDamage.damage;
            damageType = attackDamage.type;
            isMagicalDamage = AttackDamage.IsMagicAttack(damageType);
            isCrit = false;
            isHeal = false;
        }
    }
}