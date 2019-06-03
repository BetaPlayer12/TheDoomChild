public interface IShadow
{
    void BecomeAShadow(bool needsMagicToSustain);
    void BecomeNormal();
    bool hasMorphed { get; }
}
