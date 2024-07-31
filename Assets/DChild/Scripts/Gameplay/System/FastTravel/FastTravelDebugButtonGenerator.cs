using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace DChild.Gameplay.FastTravel.Debug
{
    public class FastTravelDebugButtonGenerator : MonoBehaviour
    {
        [SerializeField]
        private FastTravelDataList m_toInstantiate;
        [SerializeField]
        private GameObject m_button;
        [SerializeField]
        private Transform m_content;


        [Button]
        private void GenerateButtons()
        {
            var oldButtons = m_content.GetComponentsInChildren<FastTravelOptionButton>();
            for (int i = 0; i < oldButtons.Length; i++)
            {
                DestroyImmediate(oldButtons[i].gameObject);
            }


            for (int i = 0; i < m_toInstantiate.count; i++)
            {
                var instance = PrefabUtility.InstantiatePrefab(m_button, m_content) as GameObject;
                instance.GetComponent<FastTravelOptionButton>().SetData(m_toInstantiate.GetData(i));
            }
        }
    }
}
