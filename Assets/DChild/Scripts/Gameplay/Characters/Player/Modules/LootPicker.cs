using DChild.Gameplay.Pooling;
using DChild.Gameplay.SoulEssence;
using DChild.Gameplay.Systems;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class LootPicker : MonoBehaviour
    {
        public event EventAction<PrimitiveEventActionArgs<int>> SoulEssenceCollected;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var loot = collision.GetComponentInParent<Loot>();
            if (loot)
            {
                loot.PickUp();
                if (DChildUtility.IsSubclassOf<SoulEssenceLoot>(loot))
                {
                    var eventArgs = new PrimitiveEventActionArgs<int>();
                    eventArgs.SetValue(((SoulEssenceLoot)loot).value);
                    SoulEssenceCollected?.Invoke(this, eventArgs);
                }
            }
        }
    }

}