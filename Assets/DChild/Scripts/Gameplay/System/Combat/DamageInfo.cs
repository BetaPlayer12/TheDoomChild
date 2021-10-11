namespace DChild.Gameplay.Combat
{
    public struct DamageInfo
    {
        public Damage damage { get; set; }
        public bool isHeal { get; set; }
        public bool isMagicalDamage { get; }

        public DamageInfo(Damage attackDamage)
        {
            damage = attackDamage;
            isMagicalDamage = Damage.IsMagicDamage(damage.type);
            isHeal = false;
            //Content
        }
    }
}