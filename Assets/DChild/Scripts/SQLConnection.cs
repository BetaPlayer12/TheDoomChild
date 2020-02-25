using System.Collections;
using System.Collections.Generic;
using System.Data;
using Holysoft;
using Mono.Data.Sqlite;
using UnityEngine;

public static class SQLConnection
{
    public struct DatabaseConnection
    {
        private IDbConnection connection;

        public DatabaseConnection(IDbConnection connection)
        {
            this.connection = connection;
        }

        public IDataReader ExecuteQuery(string query)
        {
            return connection.ExecuteQuery(query);
        }

        public void ExecuteCommand(string query)
        {
            connection.ExecuteCommand(query);
        }

        public void Dispose()
        {
            connection = null;
        }
    }

    private static string path => "URI=file:" + Application.streamingAssetsPath + "/Database/";
    private static Dictionary<string, IDbConnection> connections;

    public static DatabaseConnection GetConnection(string database)
    {
        if (connections?.ContainsKey(database) ?? false)
        {
            return new DatabaseConnection(connections[database]);
        }
        throw new System.Exception("Connection is not made");
    }

    public static void Open(string database)
    {
        if (connections == null)
        {
            connections = new Dictionary<string, IDbConnection>();
        }

        if (connections.ContainsKey(database) == false)
        {
            var dbConnection = (IDbConnection)new SqliteConnection(path + database + ".db");
            dbConnection.Open();
            dbConnection.ExecuteCommand("PRAGMA foreign_keys = ON;");
            connections.Add(database, dbConnection);
        }
    }

    public static void Close(string database)
    {
        if (connections != null && connections.ContainsKey(database))
        {
            connections[database].Close();
            connections.Remove(database);
        }
    }

    public static void CloseAll()
    {
        if (connections != null)
        {
            foreach (var key in connections.Keys)
            {
                connections[key].Close();
                connections.Remove(key);
            }
        }
    }

    public static T GetData<T>(this IDataReader reader, string column)
    {
        var value = (T)reader[column];
        if (value == null)
        {
            value = default;
        }
        return value;
    }

    private static IDataReader ExecuteQuery(this IDbConnection dBconnection, string query)
    {
        var command = dBconnection.CreateCommand();
        command.CommandText = query;
        var reader = command.ExecuteReader();
        command.Dispose();
        return reader;
    }

    private static void ExecuteCommand(this IDbConnection dBconnection, string query)
    {
        var command = dBconnection.CreateCommand();
        command.CommandText = query;
        var reader = command.ExecuteNonQuery();
        command.Dispose();

    }
}
