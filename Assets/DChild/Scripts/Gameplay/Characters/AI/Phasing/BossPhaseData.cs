using Sirenix.OdinInspector;
using System;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    public interface IPhaseInfo
    {

    }

    public interface IPhaseData
    {
        IPhaseInfo info { get; }
    }

    [CreateAssetMenu(fileName = "BossPhaseData", menuName = "DChild/Gameplay/Character/Boss Phase Data")]
    public class BossPhaseData : SerializedScriptableObject, IPhaseData
    {
        [SerializeField]
        private IPhaseInfo m_info;

        public IPhaseInfo info => m_info;
    }
}