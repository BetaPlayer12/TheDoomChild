namespace DChild.Gameplay.Combat
{
    public struct BodyDefense
    {
        public BodyDefense(Invulnerability level, float damageReduction) : this()
        {
            this.damageReduction = damageReduction;
            invulnerabilityLevel = level;
        }

        public Invulnerability invulnerabilityLevel { get; }
        public float damageReduction { get; }
    }
}