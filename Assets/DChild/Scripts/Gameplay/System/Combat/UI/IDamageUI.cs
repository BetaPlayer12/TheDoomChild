using UnityEngine;

namespace DChild.Gameplay.Combat.UI
{
    public interface IDamageUI
    {
        void Load(int value, IDamageUIConfig damageUI, bool isCrit);
        void SpawnAt(Vector3 position);
    }
}