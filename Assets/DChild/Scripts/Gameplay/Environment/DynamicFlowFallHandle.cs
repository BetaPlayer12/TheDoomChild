using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class DynamicFlowFallHandle : MonoBehaviour
    {
        [SerializeField, DetailedInfoBox("DynamicFlowFalls Restrictions", "0 = Start Flow \n 1= stop Flow \n 2=Prep for next")]
        private DynamicFlowFalls m_reference;
        [SerializeField, MinValue(1)]
        private int m_clones = 1;
        [SerializeField]
        private Transform m_parent;
        [SerializeField]
        private bool m_startAsFlowing = true;
        [SerializeField]
        private bool m_autoPrepareNextFlow;

        private DynamicFlowFalls[] m_falls;
        private int m_currentIndex;
        private bool m_isblocked;

        [Button, HideIf("m_isblocked")]
        public void BlockFlow()
        {
            if (m_isblocked == false)
            {
                m_falls[m_currentIndex].ChangeInto(1);
                m_isblocked = true;
            }
        }

        [Button, ShowIf("m_isblocked")]
        public void StartFlow()
        {
            if (m_isblocked)
            {
                m_currentIndex = (int)Mathf.Repeat(m_currentIndex + 1, m_falls.Length);
                var instance = m_falls[m_currentIndex];
                instance.gameObject.SetActive(true);
                instance.ChangeInto(0);
                m_isblocked = false;
                if (m_autoPrepareNextFlow)
                {
                    PrepareNextFlow();
                }
            }
        }

        public void PrepareNextFlow()
        {
            var index = (int)Mathf.Repeat(m_currentIndex + 1, m_falls.Length);
            var instance = m_falls[index];
            instance.gameObject.SetActive(true);
            instance.ChangeInto(2);
        }

        private void Awake()
        {
            m_falls = new DynamicFlowFalls[m_clones + 1];
            m_falls[0] = m_reference;
            for (int i = 0; i < m_clones; i++)
            {
                var instance = Instantiate(m_reference.gameObject, m_parent);
                instance.SetActive(false);
                m_falls[i + 1] = instance.GetComponent<DynamicFlowFalls>();
            }

            m_reference.gameObject.SetActive(m_startAsFlowing);
            m_isblocked = !m_startAsFlowing;
            m_currentIndex = m_startAsFlowing ? 0 : -1;
            if (m_startAsFlowing)
            {
                m_currentIndex = 0;
                var instance = m_falls[m_currentIndex];
                instance.gameObject.SetActive(true);
                instance.ChangeInto(0);
                if (m_autoPrepareNextFlow)
                {
                    PrepareNextFlow();
                }
            }
        }
    }
}