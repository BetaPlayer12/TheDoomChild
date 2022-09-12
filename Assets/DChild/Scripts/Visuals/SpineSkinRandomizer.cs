using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using DChild.Menu.Bestiary;

public class SpineSkinRandomizer : MonoBehaviour
{

    [SerializeField, SpineSkin]
    private List<string> m_variation;
    [SerializeField]
    private List<BestiaryData> m_variationData;

    private SkeletonAnimation m_skeletonAnimation;

    private void Awake()
    {
        m_skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        var chosenVariationIndex = UnityEngine.Random.Range(0, m_variation.Count);
        m_skeletonAnimation.skeleton.SetSkin(m_variation[chosenVariationIndex]);
        GetComponentInChildren<BestiaryEntity>().SetData(m_variationData[chosenVariationIndex]);
    }
}
