using UnityEngine;

namespace Holysoft.UI
{
    public abstract class UIBehaviour : MonoBehaviour
    {
        public virtual void Enable()
        {
            enabled = true;
        }

        public virtual void Disable()
        {
            enabled = false;
        }
    }
}