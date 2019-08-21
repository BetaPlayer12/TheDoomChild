using TMPro;
using UnityEngine;

namespace DChild.Gameplay.Combat.UI
{
    public interface IDamageUI
    {
        void Load(int value, TMP_ColorGradient configuration, bool isCrit);
        void SpawnAt(Vector3 position);
    }
}