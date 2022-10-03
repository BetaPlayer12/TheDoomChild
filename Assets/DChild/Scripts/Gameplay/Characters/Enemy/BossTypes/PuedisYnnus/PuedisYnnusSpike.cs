using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuedisYnnusSpike : MonoBehaviour
{
    [SerializeField, Spine.Unity.SpineSkin(dataField = "m_skeletonAnimation")]
    private List<string> m_skins;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private List<string> m_startAnimations;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private List<string> m_idleAnimations;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private List<string> m_endAnimations;
}
