/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public abstract class LootChestVisual : MonoBehaviour
    {
       public abstract void Open(bool instant = false);

        public abstract void Close(bool instant = false);
    }
}