using UnityEngine;
using Spine;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System;

namespace DChild.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "ConditionalFlinchData", menuName = "DChild/Gameplay/Character/Conditional Flinch Data")]
    public class ConfigurableFlinchData : ScriptableObject
    {
        [SerializeField,EnumPaging]
        private FlinchOption m_flinchOptions;

        [SerializeField, MinValue(1), ShowIf("m_flinchOptions.HasFlag(FlinchOption.NumberOfHits)")]
        private int m_numberOfHitsRequired = 1;
        [SerializeField, MinValue(1), ShowIf("m_flinchOptions.HasFlag(FlinchOption.AmountOfDamageType)")]
        private Damage[] m_damageAmountRequired;
        [SerializeField, ShowIf("m_flinchOptions.HasFlag(FlinchOption.DuringAnimation)")]
        private SkeletonData m_skeletonData;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("m_flinchOptions.HasFlag(FlinchOption.DuringAnimation)")]
        private string[] m_onAnimationList;
    }

    [Flags]
    public enum FlinchOption
    {
        OnEveryHit = 1 << 0,
        NumberOfHits = 1 << 1,
        AmountOfDamageType = 1 << 2,
        DuringAnimation = 1 << 3
    }
}