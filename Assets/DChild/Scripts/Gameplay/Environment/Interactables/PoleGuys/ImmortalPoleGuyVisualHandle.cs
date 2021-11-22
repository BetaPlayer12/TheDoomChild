using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public class ImmortalPoleGuyVisualHandle : VisualHandle<ImmortalPoleGuyVisualData>
    {
        protected override void SetVisuals(int index)
        {
            var visualInfo = m_data.GetVisualInfo(index);
            visualInfo.LoadVisualsTo(GetComponentInChildren<SkeletonAnimation>(), GetComponentInChildren<BoxCollider2D>());
        }
    }
}