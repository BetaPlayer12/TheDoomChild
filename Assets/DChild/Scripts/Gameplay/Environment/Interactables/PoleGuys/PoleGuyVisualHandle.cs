using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;


namespace DChild.Gameplay.Environment.Interractables
{
    public class PoleGuyVisualHandle : VisualHandle<PoleGuyVisualData>
    {
        protected override void SetVisuals(int index)
        {
            var visualInfo = m_data.GetVisualInfo(index);
            visualInfo.LoadVisualsTo(GetComponentInChildren<SkeletonAnimation>(), GetComponentInChildren<BoxCollider2D>());
        }
    }
}