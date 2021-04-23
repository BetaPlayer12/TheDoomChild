using UnityEngine;
using Spine.Unity;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Environment
{
    [CreateAssetMenu(fileName = "PoisonMushroomData", menuName = "DChild/Gameplay/Environment/Poison Mushroom Data")]
    public class PoisonMushroomData : ScriptableObject
    {
        [SerializeField]
        private float m_emmisionDelayTime;
        [SerializeField]
        private float m_resetDelayTime;

        [SerializeField, BoxGroup("Animation")]
        private SkeletonDataAsset m_skeletonData;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), BoxGroup("Animation")]
        private string m_anticipationAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), BoxGroup("Animation")]
        private string m_emissionAnimation;

        [SerializeField]
        private GameObject m_anticipationFXReference;
        [SerializeField, SpineEvent(dataField = "m_skeletonData")]
        private string m_emissionFXEvent;
        [SerializeField]
        private GameObject m_emissionFXReference;

        public float emmisionDelayTime => m_emmisionDelayTime;
        public float resetDelayTime => m_resetDelayTime;

        public string anticipationAnimation => m_anticipationAnimation;
        public string emissionAnimation => m_emissionAnimation;

        public GameObject anticipationFXReference => m_anticipationFXReference;
        public string emissionFXEvent => m_emissionFXEvent;
        public GameObject emissionFXReference => m_emissionFXReference;
    }

}
