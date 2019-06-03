namespace DChild.Gameplay
{
    public interface IIsolatedTimeModifier
    {
        float slowFactor { get; }
        float fastFactor { get; }

        void StopTime();
        void ResumeTime();
        void Faster(float percentValue);
        void Slower(float percentValue);
    }
}
