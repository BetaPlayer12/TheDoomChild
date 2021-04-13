using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChildDebug
{
    public class BatchRenameHandle : MonoBehaviour
    {
        private enum FilterType
        {
            None,
            StartWith,
            Contains,
            EndWith
        }

        [SerializeField]
        private GameObject m_scope;
        [SerializeField, ShowIf("@m_scope != null")]
        private bool m_includeChildrenOfChildren;
        [SerializeField]
        private FilterType m_stringFilterType;
        [SerializeField, ShowIf("@m_stringFilterType != FilterType.None")]
        private string m_filter;
        [SerializeField, ShowIf("@m_stringFilterType != FilterType.None")]
        private bool m_isCaseSensitive;
        [SerializeField]
        private string m_prefix;
        [SerializeField, MinValue(1)]
        private int m_startingIndex;

        [Button]
        private void ExecuteRename()
        {
            List<GameObject> toEdit = new List<GameObject>();
            List<GameObject> cache = new List<GameObject>();

            //Gather
            if (m_scope)
            {
                if (m_includeChildrenOfChildren)
                {
                    cache.AddRange(m_scope.GetComponentsInChildren<Transform>().Select(x => x.gameObject).ToArray());
                }
                else
                {
                    cache.Add(m_scope);
                    for (int i = 0; i < m_scope.transform.childCount; i++)
                    {
                        cache.Add(m_scope.transform.GetChild(i).gameObject);
                    }
                }

            }
            else
            {
                cache.AddRange(FindObjectsOfType<GameObject>());
            }

            //Filter

            switch (m_stringFilterType)
            {
                case FilterType.None:
                    toEdit.AddRange(cache);
                    break;
                case FilterType.StartWith:
                    foreach (var item in cache)
                    {
                        if (m_isCaseSensitive)
                        {
                            if (item.name.StartsWith(m_filter))
                            {
                                toEdit.Add(item);
                            }
                        }
                        else if (item.name.ToLower().StartsWith(m_filter.ToLower()))
                        {
                            toEdit.Add(item);
                        }
                    }
                    break;
                case FilterType.Contains:
                    foreach (var item in cache)
                    {
                        if (m_isCaseSensitive)
                        {
                            if (item.name.Contains(m_filter))
                            {
                                toEdit.Add(item);
                            }
                        }
                        else if (item.name.ToLower().Contains(m_filter.ToLower()))
                        {
                            toEdit.Add(item);
                        }
                    }
                    break;
                case FilterType.EndWith:
                    foreach (var item in cache)
                    {
                        if (m_isCaseSensitive)
                        {
                            if (item.name.EndsWith(m_filter))
                            {
                                toEdit.Add(item);
                            }
                        }
                        else if (item.name.ToLower().EndsWith(m_filter.ToLower()))
                        {
                            toEdit.Add(item);
                        }
                    }
                    break;
            }

            for (int i = 0; i < toEdit.Count; i++)
            {
                toEdit[i].name = $"{m_prefix} ({i + m_startingIndex})";
            }
        }
    }

}