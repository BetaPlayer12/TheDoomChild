using DChild.Gameplay.Essence;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class SoulEssenceMagnet : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var soulEssence = collision.GetComponentInParent<SoulEssenceLoot>();
            if (soulEssence)
            {
                soulEssence.DisableEnvironmentCollider();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var soulEssence = collision.GetComponentInParent<SoulEssenceLoot>();
            if (soulEssence)
            {
                soulEssence.EnableEnvironmentCollider();
            }
        }
       
}

}