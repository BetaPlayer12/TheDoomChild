namespace DChild.Gameplay.Physics
{
    public interface ILinearDrag
    {
        float SetX { set; }
        float SetY { set; }
        float x { get; }
        float y { get; }
        void AddDrag(float x = float.NaN, float y = float.NaN);
        void ReduceDrag(float x = float.NaN, float y = float.NaN);
    }
}