using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    public class NavMapPOISerializationConfigurator : MonoBehaviour
    {
        [System.Serializable]
        public struct ScenePOIInfo
        {
            [SerializeField, MinValue(0)]
            public int m_sceneIndex;
            [SerializeField, MinValue(0)]
            public int m_itemCount;
        }

        [SerializeField]
        private DialogueDatabase m_database;
        [SerializeField, HorizontalGroup("NamePrefix")]
        private Environment.Location m_location;
        [SerializeField, TableList(AlwaysExpanded = true)]
        private ScenePOIInfo[] m_info;


        public ScenePOIInfo[] sceneInfo => m_info;
        public Environment.Location location => m_location;
        public DialogueDatabase database => m_database;

        [Button]
        public void SerializeToDatabase()
        {
            m_database.variables.Clear();
            var template = Template.FromDefault();
            for(int count = 0; count < m_info.Length; count++)
            {
                var info = m_info[count];
                for(int i = 0; i < info.m_itemCount; i++)
                {
                    var varID = template.GetNextVariableID(m_database);
                    var variable = template.CreateVariable(varID, CreateVariableName(info.m_sceneIndex, i), "false", FieldType.Boolean);
                    m_database.variables.Add(variable);
                }
            }
        }

        private string CreateVariableName(int sceneIndex, int index) => $"{m_location}_{sceneIndex}_POI_{index}";

    }
}

