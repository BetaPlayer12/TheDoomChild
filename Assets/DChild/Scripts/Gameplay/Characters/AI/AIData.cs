using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Refactor.DChild.Gameplay.Character.AI
{
    public interface IAIInfo { }

    [CreateAssetMenu(fileName = "AIData", menuName = "DChild/Gameplay/AI Data")]
    public class AIData : SerializedScriptableObject
    {
        [SerializeField]
        private IAIInfo m_info;

        public IAIInfo info => m_info;
    }
}