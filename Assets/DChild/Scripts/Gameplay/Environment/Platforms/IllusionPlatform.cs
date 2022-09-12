using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public abstract class IllusionPlatform : MonoBehaviour
    {
        public abstract void Appear(bool instant);
        public abstract void Disappear(bool instant);
    }
}
