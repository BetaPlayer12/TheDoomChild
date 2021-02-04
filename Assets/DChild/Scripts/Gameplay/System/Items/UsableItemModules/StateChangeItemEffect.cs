using DChild.Gameplay.Characters.Players;

namespace DChild.Gameplay.Items
{
    public struct StateChangeItemEffect : IDurationItemEffect
    {
        private enum State
        {
            Rage
        }

        public void StartEffect(IPlayer player)
        {
            throw new System.NotImplementedException();
        }

        public void StopEffect(IPlayer player)
        {
            throw new System.NotImplementedException();
        }
    }
}
