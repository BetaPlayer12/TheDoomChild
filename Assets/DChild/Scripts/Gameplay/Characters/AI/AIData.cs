using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Refactor.DChild.Gameplay.Characters.AI
{
    public interface IAIInfo { void Initialize(); }

    [CreateAssetMenu(fileName = "AIData", menuName = "DChild/Gameplay/AI Data")]
    public class AIData : SerializedScriptableObject
    {

        [SerializeField]
        private IAIInfo m_info;

        public IAIInfo info => m_info;


#if UNITY_EDITOR
        [Button, PropertyOrder(-1)]
        private void Referesh()
        {
            m_info.Initialize();
        }
        private void OnEnable()
        {
            m_info.Initialize();
        }
#endif
    }
}