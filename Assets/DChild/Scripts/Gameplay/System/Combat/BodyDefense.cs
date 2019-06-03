namespace DChild.Gameplay.Combat
{
    public struct BodyDefense
    {
        public BodyDefense(float damageReduction) : this()
        {
            this.damageReduction = damageReduction;
            isInvulnerable = false;
        }

        public BodyDefense(bool value = true) : this()
        {
            isInvulnerable = value;
            this.damageReduction = 0;
        }

        public bool isInvulnerable { get; }
        public float damageReduction { get; }
    }
}