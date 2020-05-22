using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class WaterWheelBlocker : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponentInParent(out WaterfallWheelHandle handle))
            {
                handle.BlockWater();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponentInParent(out WaterfallWheelHandle handle))
            {
                handle.UnblockWater();
            }
        }
    }
}
