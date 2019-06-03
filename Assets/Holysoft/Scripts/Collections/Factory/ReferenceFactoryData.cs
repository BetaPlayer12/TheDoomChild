using UnityEngine;
using Sirenix.OdinInspector;

namespace Holysoft.Collections
{
    public class ReferenceFactoryData : MonoBehaviour, IReferenceFactoryData
    {
        [SerializeField]
        [MinValue(1)]
        private int m_instanceCount;

        public int instanceCount => m_instanceCount;
    }
}