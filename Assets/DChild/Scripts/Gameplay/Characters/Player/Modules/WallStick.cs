using DChild.Gameplay.Characters.Players.Behaviour;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class WallStick : MonoBehaviour, ICancellableBehaviour
    {
        public void Cancel()
        {
            throw new System.NotImplementedException();
        }

        public bool IsThereAWall()
        {
            return true;
        }

        public void Execute()
        {

        }

    }
}
