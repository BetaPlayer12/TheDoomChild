using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DChild.Serialization
{
    [System.Serializable, HideLabel]
    public struct SerializeID
    {
        public struct EqualityComparer : IEqualityComparer<SerializeID>
        {
            public bool Equals(SerializeID x, SerializeID y)
            {
                return x.value == y.value;
            }

            public int GetHashCode(SerializeID obj)
            {
                return obj.value.GetHashCode();
            }
        }

        [SerializeField, HideIf("m_isOnAppend"), ValueDropdown("GetIDs"), HorizontalGroup("ID")]
        private int m_ID;
        [SerializeField, HideInInspector]
        private bool m_showHandle;
        public SerializeID(bool showHandle) : this()
        {
            m_showHandle = showHandle;
        }

        public SerializeID(SerializeID reference, bool showHandle) : this()
        {
            m_ID = reference.m_ID;
            m_showHandle = showHandle;
        }

        public SerializeID(int iD) : this()
        {
            m_ID = iD;
        }

        public int value => m_ID;

        public static int defaultValue => 0;

        public static implicit operator int(SerializeID id)
        {
            return id.value;
        }

#if UNITY_EDITOR

        private bool m_isOnAppend;
        private int m_createdID;
        private bool m_canCreateID;
        [ShowInInspector, ValidateInput("ValidateContext"), ShowIf("m_isOnAppend"), OnValueChanged("ConfirmChange")]
        private string m_context;


        private IEnumerable GetIDs()
        {
            ValueDropdownList<int> list = new ValueDropdownList<int>();
            var connection = DChildDatabase.GetSerializeIDConnection();
            connection.Initialize();
            var elements = connection.GetAll();
            for (int i = 0; i < elements.Count; i++)
            {
                list.Add(new ValueDropdownItem<int>(elements[i].name, elements[i].id));
            }
            connection.Close();

            if (list.Count == 0)
            {
                m_isOnAppend = true;
            }

            return list;
        }

        [Button, HorizontalGroup("ID"), ShowIf("@m_showHandle && !m_isOnAppend"), HideInPlayMode]
        private void CreateID()
        {
            m_isOnAppend = true;
            var connection = DChildDatabase.GetSerializeIDConnection();
            connection.Initialize();
            m_createdID = connection.GetNextID();
            connection.Close();
        }


        private bool ValidateContext(string context, ref string message)
        {
            if (string.IsNullOrEmpty(context))
            {
                m_canCreateID = false;
                message = "Context Should not be Empty";
                return false;
            }
            else
            {

                var connection = DChildDatabase.GetSerializeIDConnection();
                connection.Initialize();
                bool isUnique = !connection.DoesContextExist(context);
                connection.Close();
                if (isUnique == false)
                {
                    message = "Duplicate Context";
                }
                return isUnique;
            }
        }

        private void ConfirmChange()
        {
            string dummy = "";
            m_canCreateID = ValidateContext(m_context, ref dummy);
        }


        [Button("Cancel"), HorizontalGroup("Buttons"), ShowIf("m_isOnAppend")]
        private void CancelCreateID()
        {
            m_isOnAppend = false;
            m_context = string.Empty;
            m_canCreateID = false;
        }

        [Button("Create"), HorizontalGroup("Buttons"), ShowIf("@m_canCreateID && m_isOnAppend")]
        private void AppendToDataBase()
        {
            m_ID = m_createdID;

            var connection = DChildDatabase.GetSerializeIDConnection();
            connection.Initialize();
            connection.Append(m_ID, m_context);
            connection.Close();
            CancelCreateID();
        }
#endif
    }
}