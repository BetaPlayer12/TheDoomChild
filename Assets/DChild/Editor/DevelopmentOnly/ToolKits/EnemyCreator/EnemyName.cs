using DChild;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChildEditor.Toolkit.EnemyCreation
{
    [System.Serializable]
    public class EnemyNameField
    {
        [SerializeField, LabelText("Name"), LabelWidth(40), ShowIf("m_useCustomName")/*, HorizontalGroup("Split")*/]
        private string m_customName;
        [SerializeField, LabelText("Name"), LabelWidth(40), HideIf("m_useCustomName"), ValueDropdown("GetNames")/*, HorizontalGroup("Split")*/]
        private string m_databaseName;
        [SerializeField, HorizontalGroup("Split")]
        private bool m_useCustomName;

        public string value => m_useCustomName ? m_customName.Replace(" ", string.Empty) : m_databaseName.Replace(" ", string.Empty);

        private IEnumerable GetNames()
        {
            var connection = DChildDatabase.GetBestiaryConnection();
            connection.Initialize();
            var infoList = connection.GetAllInfo();
            connection.Close();

            var list = new ValueDropdownList<string>();
            for (int i = 0; i < infoList.Length; i++)
            {
                list.Add(infoList[i].name);
            }
            return list;
        }
    }
}