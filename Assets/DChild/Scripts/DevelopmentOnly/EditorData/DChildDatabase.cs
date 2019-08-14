using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace DChild
{
    public interface IDatabaseConnection
    {
        void Initialize();
        void Close();
    }

    [System.Serializable]
    public static class DChildDatabase
    {
        #region Soul Skill
        public struct SoulSkillConnection : IDatabaseConnection
        {
            public struct Element
            {
                public Element(int id, string name) : this()
                {
                    this.id = id;
                    this.name = name;
                }

                public int id { get; }
                public string name { get; }
            }

            private SQLConnection.DatabaseConnection m_connection;
            private bool m_connectionOpened;
            private static string database => "DChildSetup";

            public void Initialize()
            {
                if (m_connectionOpened == false)
                {
                    SQLConnection.Open(database);
                    m_connection = SQLConnection.GetConnection(database);
                    m_connectionOpened = true;
                }
            }

            public void Close()
            {
                if (m_connectionOpened)
                {
                    SQLConnection.Close(database);
                    m_connection.Dispose();
                    m_connectionOpened = false;
                }
            }

            public string GetNameOf(int ID)
            {
                var bestiaryReader = m_connection.ExecuteQuery($"SELECT Name FROM SoulSkills WHERE ID = {ID}");
                if (bestiaryReader.Read())
                {
                    return bestiaryReader.GetData<string>("Name");
                }
                else
                {
                    return "Not Found";
                }
            }

            public Element[] GetAllSkills()
            {
                var reader = m_connection.ExecuteQuery($"SELECT ID,Name FROM SoulSkills Where Blocked = false");
                return CreateListFromReader(reader);
            }

            public Element[] GetSkillsOfType(SoulSkillType type)
            {
                var reader = m_connection.ExecuteQuery($"SELECT ID,Name FROM SoulSkills Where Type = \"{type.ToString()}\" AND Blocked = false");
                return CreateListFromReader(reader);
            }

            private Element[] CreateListFromReader(IDataReader reader)
            {
                List<Element> list = new List<Element>();
                while (reader.Read())
                {
                    list.Add(new Element(reader.GetData<int>("ID"), reader.GetData<string>("Name")));

                }
                return list.ToArray();
            }
        }
        private static SoulSkillConnection soulSkillConnection = new SoulSkillConnection();
        public static SoulSkillConnection GetSoulSkillConnection() => soulSkillConnection;
        #endregion

        #region Bestiary
        public struct BestiaryConnection
        {
            public struct CoreInfo
            {
                public CoreInfo(int iD, string name) : this()
                {
                    ID = iD;
                    this.name = name;
                }

                public int ID { get; }
                public string name { get; }
            }

            private SQLConnection.DatabaseConnection m_connection;
            private bool m_connectionOpened;
            private static string database => "DChildSetup";

            public void Initialize()
            {
                if (m_connectionOpened == false)
                {
                    SQLConnection.Open(database);
                    m_connection = SQLConnection.GetConnection(database);
                    m_connectionOpened = true;
                }
            }

            public void Close()
            {
                if (m_connectionOpened)
                {
                    SQLConnection.Close(database);
                    m_connection.Dispose();
                    m_connectionOpened = false;
                }
            }

            public CoreInfo[] GetAllInfo()
            {
                List<CoreInfo> list = new List<CoreInfo>();
                var bestiaryReader = m_connection.ExecuteQuery($"SELECT * FROM Bestiary");
                while (bestiaryReader.Read())
                {
                    list.Add(new CoreInfo(bestiaryReader.GetData<int>("ID"), bestiaryReader.GetData<string>("Name")));
                }
                return list.ToArray();
            }

            public string GetNameOf(int ID)
            {
                var bestiaryReader = m_connection.ExecuteQuery($"SELECT Name FROM Bestiary WHERE ID = {ID}");
                if (bestiaryReader.Read())
                {
                    return bestiaryReader.GetData<string>("Name");
                }
                else
                {
                    return "Not Found";
                }
            }

            public (string description, Location[] locations) GetInfo(int ID)
            {
                var bestiaryReader = m_connection.ExecuteQuery($"SELECT * FROM Bestiary Where ID = {ID}");
                if (bestiaryReader.Read())
                {
                    List<Location> locationList = new List<Location>();
                    var locationReader = m_connection.ExecuteQuery($"SELECT * FROM Bestiary_Location Where Bestiary_ID = {ID}");
                    while (locationReader.Read())
                    {
                        Location result;
                        if (Enum.TryParse(locationReader.GetData<string>("Location"), out result))
                        {
                            locationList.Add(result);
                        }
                    }

                    return (bestiaryReader.GetData<string>("Description"), locationList.Count > 0 ? locationList.ToArray() : null);
                }
                else
                {
                    return ("", null);
                }
            }

            public void UpdateLocation(int ID, Location[] locations)
            {
                m_connection.ExecuteCommand($"DELETE FROM Bestiary_Location WHERE Bestiary_ID = {ID}");
                if (locations.Length > 0)
                {
                    for (int i = 0; i < locations.Length; i++)
                    {
                        m_connection.ExecuteCommand($"INSERT INTO Bestiary_Location VALUES({ID},\"{locations[i].ToString()}\")");
                    }
                }
            }

            public void Update(int ID, string name, string description)
            {
                var bestiaryReader = m_connection.ExecuteQuery($"SELECT * FROM Bestiary Where ID = {ID}");
                if (bestiaryReader.Read())
                {
                    m_connection.ExecuteCommand($"UPDATE Bestiary SET Name = \"{name}\",  Description = \"{description}\"  WHERE ID = {ID}");
                }
                else
                {
                    throw new System.Exception($"Record with ID:{ID} does not exists, use Insert instead");
                }
            }

            public void UpdateID(string reference, int ID)
            {
                m_connection.ExecuteCommand($"UPDATE Bestiary SET ID = {ID} WHERE Name = \"{reference}\"");
            }

            public void Insert(int ID, string name, string description)
            {
                var bestiaryReader = m_connection.ExecuteQuery($"SELECT * FROM Bestiary Where ID = {ID}");
                if (bestiaryReader.Read())
                {
                    throw new System.Exception($"Record with ID:{ID} already exists, use Update instead");
                }
                else
                {
                    m_connection.ExecuteCommand($"INSERT INTO Bestiary VALUES({ID},\"{name}\",\"{description}\")");

                }
            }

            public void Delete(params int[] IDs)
            {
                for (int i = 0; i < IDs.Length; i++)
                {
                    var ID = IDs[i];
                    m_connection.ExecuteCommand($"DELETE FROM Bestiar WHERE ID = {ID}");
                    m_connection.ExecuteCommand($"DELETE FROM Bestiary_Location WHERE Bestiary_ID = {ID}");
                }
            }
        }
        private static BestiaryConnection bestiaryConnection = new BestiaryConnection();
        public static BestiaryConnection GetBestiaryConnection() => bestiaryConnection;
        #endregion

        #region Item
        public struct ItemConnection
        {
            public struct CoreInfo
            {
                public CoreInfo(int iD, string name) : this()
                {
                    ID = iD;
                    this.name = name;
                }

                public int ID { get; }
                public string name { get; }
            }

            private SQLConnection.DatabaseConnection m_connection;
            private bool m_connectionOpened;
            private static string database => "DChildSetup";
            private static string table => "Items";

            public void Initialize()
            {
                if (m_connectionOpened == false)
                {
                    SQLConnection.Open(database);
                    m_connection = SQLConnection.GetConnection(database);
                    m_connectionOpened = true;
                }
            }

            public void Close()
            {
                if (m_connectionOpened)
                {
                    SQLConnection.Close(database);
                    m_connection.Dispose();
                    m_connectionOpened = false;
                }
            }

            public CoreInfo[] GetAllInfo()
            {
                List<CoreInfo> list = new List<CoreInfo>();
                var reader = m_connection.ExecuteQuery($"SELECT * FROM {table}");
                while (reader.Read())
                {
                    list.Add(new CoreInfo(reader.GetData<int>("ID"), reader.GetData<string>("Name")));
                }
                return list.ToArray();
            }

            public string GetNameOf(int ID)
            {
                var reader = m_connection.ExecuteQuery($"SELECT Name FROM {table} WHERE ID = {ID}");
                if (reader.Read())
                {
                    return reader.GetData<string>("Name");
                }
                else
                {
                    return "Not Found";
                }
            }

            public (string description, int quantityLimit, int cost) GetInfoOf(int ID)
            {
                var reader = m_connection.ExecuteQuery($"SELECT * FROM {table} WHERE ID = {ID}");
                if (reader.Read())
                {
                    return (reader.GetData<string>("Description"), reader.GetData<int>("MaxStack"), reader.GetData<int>("Cost"));
                }
                else
                {
                    throw new System.Exception($"Record with ID:{ID} does not exists");
                }
            }

            public void Update(int ID, string description, int quantityLimit, int cost)
            {
                var reader = m_connection.ExecuteQuery($"SELECT * FROM {table} WHERE ID ={ID}");
                if (reader.Read())
                {
                    m_connection.ExecuteCommand($"UPDATE {table} SET Description = \"{description}\", MaxStack = {quantityLimit}, Cost = {cost} WHERE ID ={ID}");
                }
                else
                {
                    throw new System.Exception($"Record with ID:{ID} does not exists, use Insert instead");
                }
            }
        }
        private static ItemConnection itemConnection = new ItemConnection();
        public static ItemConnection GetItemConnection() => itemConnection;
        #endregion
    }
}