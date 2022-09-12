using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    public class LinkAndChain : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private Rigidbody2D m_anchoredBody;

        [SerializeField, PropertySpace]
        private GameObject m_chainTemplate;
        [HorizontalGroup("ChainCount")]
        [SerializeField, HorizontalGroup("ChainCount/Count", PaddingRight = 40f), ReadOnly, HideInPrefabAssets]
        private int m_chainCount;
        [SerializeField, OnValueChanged("ArrangeChains")]
        private Vector2 m_chainOffset;

        [SerializeField, PropertySpace, OnValueChanged("LinkEndHingeToLastChain")]
        private HingeJoint2D m_endHinge;
        [SerializeField, ShowIf("@m_endHinge != null"), OnValueChanged("LinkEndHingeToLastChain")]
        private Vector2 m_endHingeOffset;


        [SerializeField, PropertySpace, OnValueChanged("LinkEndWeightToLastJoint")]
        private HingeJoint2D m_endWeight;
        [SerializeField, ShowIf("@m_endWeight != null"), OnValueChanged("LinkEndWeightToLastJoint")]
        private Vector2 m_endWeightOffset;

        [SerializeField, HideInInspector]
        private List<GameObject> m_chains = new List<GameObject>();


        [Button("-"), HorizontalGroup("ChainCount/Option"), HideInPrefabAssets]
        private void ReduceChain()
        {
            ValidateChainList();
            if (m_chainCount == 0)
                return;

            DestroyImmediate(m_chains[m_chainCount - 1]);
            m_chainCount--;
            m_chains.RemoveAt(m_chainCount);
            ArrangeChains();
        }

        [Button("+"), HorizontalGroup("ChainCount/Option"), HideInPrefabAssets]
        private void AddChain()
        {
            ValidateChainList();

            var chain = (GameObject)PrefabUtility.InstantiatePrefab(m_chainTemplate, transform);
            m_chainCount++;
            m_chains.Add(chain);
            ArrangeChains();
        }


        [Button]
        private void ArrangeChains()
        {
            ValidateChainList();

            var firstChain = m_chains[0];
            firstChain.name = m_chainTemplate.name + "1";
            firstChain.transform.SetSiblingIndex(0);
            LinkChainToRigidbody(firstChain, m_anchoredBody);
            Rigidbody2D previousRigidbody = firstChain.GetComponent<Rigidbody2D>();
            if (m_chainCount > 1)
            {
                for (int i = 1; i < m_chains.Count; i++)
                {
                    var chain = m_chains[i];
                    chain.transform.SetSiblingIndex(i);
                    chain.name = m_chainTemplate.name + (i + 1);
                    chain.transform.position = previousRigidbody.transform.position + (Vector3)m_chainOffset;

                    LinkChainToRigidbody(chain, previousRigidbody);
                    previousRigidbody = chain.GetComponent<Rigidbody2D>();
                }
            }

            if (m_endHinge != null)
            {
                var lastChain = m_chains[m_chainCount - 1];
                m_endHinge.transform.SetAsLastSibling();
                LinkEndHinge(m_endHinge, previousRigidbody, lastChain.transform, m_endHingeOffset);
            }

            if (m_endWeight != null)
            {
                m_endWeight.transform.SetAsLastSibling();
                LinkEndWeightToLastJoint();
            }
        }
        private void LinkEndHingeToLastChain()
        {
            var lastChain = m_chains[m_chainCount - 1];
            LinkEndHinge(m_endHinge, lastChain.GetComponent<Rigidbody2D>(), lastChain.transform, m_endHingeOffset);
        }

        private void LinkEndWeightToLastJoint()
        {
            if (m_endWeight != null)
            {
                LinkEndHinge(m_endWeight, m_endHinge.GetComponent<Rigidbody2D>(), m_endHinge.transform, m_endWeightOffset);
            }
            else
            {
                var lastChain = m_chains[m_chainCount - 1];
                LinkEndHinge(m_endWeight, lastChain.GetComponent<Rigidbody2D>(), lastChain.transform, m_endWeightOffset);
            }
        }

        private void LinkEndHinge(HingeJoint2D hinge, Rigidbody2D previousRigidbody, Transform positionReference, Vector3 offset)
        {
            hinge.connectedBody = previousRigidbody;
            hinge.transform.SetSiblingIndex(m_chainCount);
            hinge.transform.position = positionReference.position + (Vector3)offset;
        }

        private void LinkChainToRigidbody(GameObject chain, Rigidbody2D rigidbody)
        {
            var joint = chain.GetComponent<HingeJoint2D>();
            joint.connectedBody = rigidbody;
            joint.autoConfigureConnectedAnchor = true;
        }

        private void ValidateChainList()
        {
            m_chains.RemoveAll(x => x == null);
            m_chainCount = m_chains.Count;
        }

        private void ReplaceAllChains()
        {
            var originalChainCount = m_chainCount;
            for (int i = m_chains.Count - 1; i >= 0; i--)
            {
                Destroy(m_chains[i]);
                m_chains.RemoveAt(i);
            }
            m_chainCount = 0;
            for (int i = 0; i < originalChainCount; i++)
            {
                AddChain();
            }
        }

        private void Reset()
        {
            for (int i = m_chains.Count - 1; i >= 0; i--)
            {
                Destroy(m_chains[i]);
                m_chains.RemoveAt(i);
            }
        }
#endif
    }
}
