using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Spine.Tests
{
    public class MinionModelStressTestManager : StressTestManager<GameObject>
    {
        [SerializeField]
        private MinionModelStressTestInstantiator m_instantiator;

        [SerializeField]
        private List<GameObject> m_minionPrefabs;

        protected override IStressTestInstantiator<GameObject> instantiator => m_instantiator;

        protected override IEnumerator TestAllElementsRoutine()
        {
            for (int i = 0; i < m_minionPrefabs.Count; i++)
            {
                var minion = m_minionPrefabs[i];
                yield return StressTestRoutine(minion, minion.name);
            }
        }
    }
}