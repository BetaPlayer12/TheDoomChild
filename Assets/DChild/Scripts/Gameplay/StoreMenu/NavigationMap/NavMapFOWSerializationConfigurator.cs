using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.NavigationMap
{
    public class NavMapFOWSerializationConfigurator : MonoBehaviour
    {
        [System.Serializable]
        public struct SceneFOWInfo
        {
            [SerializeField, MinValue(0)]
            public int m_sceneIndex;
            [SerializeField, MinValue(0)]
            public int m_itemCount;
        }

        [SerializeField]
        private DialogueDatabase m_database;
        [SerializeField, HorizontalGroup("NamePrefix")]
        private Environment.Location m_sceneLocation;
        [SerializeField, TableList(AlwaysExpanded = true)]
        private SceneFOWInfo[] m_info;

        public SceneFOWInfo[] sceneInfo => m_info;
        public Environment.Location location => m_sceneLocation;
        public DialogueDatabase database => m_database;



        [Button]
        public void SerializeToDatabase()
        {
            for (int y = 0; y < m_database.variables.Count; y++)
            {

                if (m_database.variables[y].fields[0].value.Contains("FOW"))
                {
                    m_database.variables.Remove(m_database.variables[y]);
                    y--;
                    m_database.SyncVariables();
                }
                else
                {

                }
            }
            var template = Template.FromDefault();
            for (int k = 0; k < m_info.Length; k++)
            {
                var info = m_info[k];
                for (int i = 0; i < info.m_itemCount; i++)
                {
                    var varID = template.GetNextVariableID(m_database);
                    var variable = template.CreateVariable(varID, CreateVariableName(info.m_sceneIndex, i), "false", FieldType.Boolean);
                    m_database.variables.Add(variable);
                }
            }
        }

        private string CreateVariableName(int sceneIndex, int index) => $"{m_sceneLocation}_{sceneIndex}_FOW_{index}";

    }
}