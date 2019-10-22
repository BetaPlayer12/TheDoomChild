using UnityEngine;
#if UNITY_EDITOR
#endif
namespace DChild.Gameplay.Items
{
    [CreateAssetMenu(fileName = "ItemList", menuName = "DChild/Database/Item List")]
    public class ItemList : DatabaseAssetList<ItemData>
    {
    }
}
