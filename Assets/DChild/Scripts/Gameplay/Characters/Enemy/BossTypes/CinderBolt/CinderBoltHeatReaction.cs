using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public abstract class CinderBoltHeatReaction : MonoBehaviour
    {
        public abstract void HandleReaction(SpineAnimation spineAnimation, int heatValue);
    }
}