using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace DChildDebug
{
    public class DatabaseUtility : SerializedMonoBehaviour
    {
        [SerializeField]
        public abstract class Handler
        {
            [SerializeField]
            private string m_name;

            protected abstract string table { get; }

            [Button]
            private void Insert()
            {
                SQLConnection.Open("DChildSetup");
                var connection = SQLConnection.GetConnection("DChildSetup");
                int id = 0;
                IDataReader reader = null;
                do
                {
                    id = GenerateID();
                    reader = connection.ExecuteQuery($"Select * FROM {table} WHERE ID = {id}");
                } while (reader.Read());
                connection.ExecuteCommand($"INSERT INTO {table} (ID, Name) VALUES ({id}, \"{m_name}\")");
                connection.Dispose();
            }

            private int GenerateID() => Random.Range(0, 100000);
        }
        [SerializeField]
        private Handler m_handler;

        [Button]
        private void RandomizeID()
        {
            SQLConnection.Open("DChildSetup");
            var connection = SQLConnection.GetConnection("DChildSetup");
            IDataReader reader = connection.ExecuteQuery($"SELECT Name From SoulSkills");
            IDataReader IDCheckReader = null;
            while (reader.Read())
            {
                int id = 0;
                do
                {
                    id = GenerateID();
                    IDCheckReader = connection.ExecuteQuery($"Select * FROM SoulSkills WHERE ID = {id}");
                } while (IDCheckReader.Read());
                string name = (string)reader["Name"];
                connection.ExecuteCommand($"UPDATE SoulSkills SET ID ={id} WHERE Name = \"{name}\"");
            }
            connection.Dispose();
        }

        private int GenerateID() => Random.Range(0, 100000);
    }

    public class BestiaryHandler : DatabaseUtility.Handler
    {
        protected override string table => "Bestiary";
    }

    public class SoulSkillsHandler : DatabaseUtility.Handler
    {
        protected override string table => "SoulSkills";
    }
    public class ItemsHandler : DatabaseUtility.Handler
    {
        protected override string table => "Items";
    }
}