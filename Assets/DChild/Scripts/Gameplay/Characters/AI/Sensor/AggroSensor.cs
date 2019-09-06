namespace DChild.Gameplay.Characters.AI
{
    public abstract class AggroSensor : AISensor
    {
        protected IAITargetingBrain m_brain;

        protected void Start()
        {
            m_brain = GetComponentInParent<IAITargetingBrain>();
        }
    }
}