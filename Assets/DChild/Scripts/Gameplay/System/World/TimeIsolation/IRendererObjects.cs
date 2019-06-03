using UnityEngine;

namespace DChild.Gameplay.Systems.WorldComponents
{
    public interface IRendererObjects
    {
        Material[] materials { get; }
        int[] countPerMaterial { get; }
    }
}