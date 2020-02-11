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
            private static string table => "SoulSkills";

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
                var bestiaryReader = m_connection.ExecuteQuery($"SELECT Name FROM {table} WHERE ID = {ID}");
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
                var reader = m_connection.ExecuteQuery($"SELECT ID,Name FROM {table} Where Blocked = false");
                return CreateListFromReader(reader);
            }

            public Element[] GetSkillsOfType(SoulSkillType type)
            {
                var reader = m_connection.ExecuteQuery($"SELECT ID,Name FROM {table} Where Type = \"{type.ToString()}\" AND Blocked = false");
                return CreateListFromReader(reader);
            }

            public void Update(int ID, SoulSkillType type, string description)
            {
                var reader = m_connection.ExecuteQuery($"SELECT * FROM {table} WHERE ID ={ID}");
                if (reader.Read())
                {
                    m_connection.ExecuteCommand($"UPDATE {table} SET Description = \"{description}\", Type = \"{type.ToString()}\" WHERE ID = {ID}");
                }
                else
                {
                    throw new System.Exception($"Record with ID:{ID} does not exists, use Insert instead");
                }
            }

            public (SoulSkillType type, string description) GetInfoOf(int ID)
            {
                var reader = m_connection.ExecuteQuery($"SELECT * FROM {table} WHERE ID = {ID}");
                if (reader.Read())
                {
                    SoulSkillType result;
                    Enum.TryParse(reader.GetData<string>("Type"), true, out result);
                    return (result, reader.GetData<string>("Description"));
                }
                else
                {
                    throw new System.Exception($"Record with ID:{ID} does not exists");
                }
            }

            public int Insert(int ID, string name, string description, SoulSkillType type)
            {
                var reader = m_connection.ExecuteQuery($"SELECT * FROM {table} WHERE ID ={ID}");
                if (reader.Read())
                {
                    //ChangeID and try Insert Again
                    return Insert((int)UnityEngine.Random.Range(0, 999999), name, description, type);
                }
                else
                {
                    m_connection.ExecuteCommand($"INSERT INTO {table} (ID,Name, Description, Type) VALUES({ID},\"{name}\",\"{description}\",\"{type.ToString()}\");");
                    return ID;
                }
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
                public CoreInfo(int iD, string name, string title) : this()
                {
                    this.ID = iD;
                    this.name = name;
                    this.title = title;
                }

                public int ID { get; }
                public string name { get; }

                public string title { get; }
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
                    list.Add(new CoreInfo(bestiaryReader.GetData<int>("ID"), bestiaryReader.GetData<string>("Name"), bestiaryReader.GetData<string>("Title")));
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

            public void Update(int ID, string name, string title, string description)
            {
                var bestiaryReader = m_connection.ExecuteQuery($"SELECT * FROM Bestiary Where ID = {ID}");
                if (bestiaryReader.Read())
                {
                    m_connection.ExecuteCommand($"UPDATE Bestiary SET Name = \"{name}\", Title = \"{(title == string.Empty? "": title)}\",  Description = \"{(description == string.Empty? "" : description)}\"  WHERE ID = {ID}");
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

            public void UpdateID(int reference, int ID)
            {
                m_connection.ExecuteCommand($"UPDATE Bestiary SET ID = {ID} WHERE ID = {reference}");
            }

            public void Insert(int ID, string name, string title, string description)
            {
                var bestiaryReader = m_connection.ExecuteQuery($"SELECT * FROM Bestiary Where ID = {ID}");
                if (bestiaryReader.Read())
                {
                    throw new System.Exception($"Record with ID:{ID} already exists, use Update instead");
                }
                else
                {
                    m_connection.ExecuteCommand($"INSERT INTO Bestiary VALUES({ID},\"{name}\",\"{title}\",\"{description}\",1,1)");

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

            public (int HP, int DMG) GetRatings(int ID)
            {
                var bestiaryReader = m_connection.ExecuteQuery($"SELECT * FROM Bestiary Where ID = {ID}");
                if (bestiaryReader.Read())
                {
                    return (bestiaryReader.GetData<int>("HP Rating"), bestiaryReader.GetData<int>("DMG Rating"));

                }
                else
                {
                    return (-1, -1);

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

            public int Insert(int ID, string name, string description, int quantityLimit, int cost)
            {
                var reader = m_connection.ExecuteQuery($"SELECT * FROM {table} WHERE ID ={ID}");
                if (reader.Read())
                {
                    //ChangeID and try Insert Again
                    return Insert((int)UnityEngine.Random.Range(0, 999999), name, description, quantityLimit, cost);
                }
                else
                {
                    m_connection.ExecuteCommand($"INSERT INTO {table} (ID,Name, Description, MaxStack,Cost) VALUES({ID},\"{name}\",\"{description}\",{quantityLimit},{cost});");
                    return ID;
                }
            }
        }
        private static ItemConnection itemConnection = new ItemConnection();
        public static ItemConnection GetItemConnection() => itemConnection;
        #endregion

        #region SerializeIDs
        public struct SerializeIDConnection
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
            private static string table => "SerializeIDs";

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

            public IReadOnlyList<Element> GetAll()
            {
                List<Element> list = new List<Element>();

                var reader = m_connection.ExecuteQuery($"SELECT * FROM {table}");

                while (reader.Read())
                {
                    list.Add(new Element(reader.GetData<int>("ID"), reader.GetData<string>("Context")));
                }

                return list;
            }

            public bool DoesContextExist(string context)
            {
                var reader = m_connection.ExecuteQuery($"SELECT * FROM {table} WHERE Context = \"{context}\"");
                return reader.Read();
            }

            public string GetContext(int ID)
            {
                var reader = m_connection.ExecuteQuery($"SELECT * FROM {table} WHERE ID = {ID}");
                if (reader.Read())
                {
                    return reader.GetData<string>("Context");

                }
                else
                {
                    return string.Empty;
                }
            }

            public int GetNextID()
            {
                List<int> list = new List<int>();

                var reader = m_connection.ExecuteQuery($"SELECT ID FROM {table}");

                while (reader.Read())
                {
                    list.Add(reader.GetData<int>("ID"));
                }

                return list.Count > 0 ? (list.Max() + 1) : 1;
            }

            public void Append(int ID, string context)
            {
                m_connection.ExecuteCommand($"INSERT INTO {table} VALUES({ID},\"{context}\")");
            }

            public void Modify(int ID, string context)
            {
                m_connection.ExecuteCommand($"UPDATE {table} SET Context = \"{context}\" WHERE ID = {ID}");
            }
        }

        private static SerializeIDConnection serializeIDConnection = new SerializeIDConnection();
        public static SerializeIDConnection GetSerializeIDConnection() => serializeIDConnection;
        #endregion
    }
}