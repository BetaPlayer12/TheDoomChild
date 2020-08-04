using DChild.Gameplay.Characters.Players.Behaviour;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class Dash : MonoBehaviour, IResettableBehaviour, ICancellableBehaviour
    {
        public void Cancel()
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public void Execute()
        {

        }
    }
}
