using DChild.Gameplay.Characters.Players.Behaviour;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class Crouch : MonoBehaviour, ICancellableBehaviour
    {
        public void Cancel()
        {
        }

        public bool IsThereNoCeiling()
        {
            return true;
        }

        public void Execute()
        {

        }
    }
}
