using Holysoft.Collections;
using Holysoft.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Menu.Extras
{
    public class MultipleReferenceFactoryData : MonoBehaviour, IReferenceFactoryData, IScrollViewContentGrid
    {
        [SerializeField]
        [ReadOnly]
        private int m_highestInstanceCount;

        public int instanceCount
        {
            get
            {
                var referenceFactoryData = new List<IReferenceFactoryData>(GetComponentsInChildren<IReferenceFactoryData>());
                referenceFactoryData.Remove(this);
                if (referenceFactoryData.Count != 0)
                {
                    var instanceCounts = referenceFactoryData.Select(x => x.instanceCount);
                    m_highestInstanceCount = instanceCounts.Max();
                    return m_highestInstanceCount;
                }
                else
                {
                    return 1;
                }
            }
        }

        public int instancePerRow => instanceCount;

        public bool restrictInstancePerRow => false;
    }

}