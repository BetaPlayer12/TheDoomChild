namespace DChild.Gameplay.Characters.Players
{
    public interface IDashModifier
    {
        float dashDistance { get; set; }
        float dashCooldown { get; set; }
    }
}