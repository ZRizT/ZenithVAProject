using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using ZenithVirtualAssistant.Models;

namespace ZenithVirtualAssistant.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            _connectionString = "Data Source=zenith.db";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS CommandHistory (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Command TEXT,
                    Response TEXT,
                    Timestamp DATETIME
                )";
            command.ExecuteNonQuery();
        }

        public void SaveCommand(string command, string response)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO CommandHistory (Command, Response, Timestamp) VALUES ($command, $response, $timestamp)";
            cmd.Parameters.AddWithValue("$command", command);
            cmd.Parameters.AddWithValue("$response", response);
            cmd.Parameters.AddWithValue("$timestamp", DateTime.Now);
            cmd.ExecuteNonQuery();
        }

        public List<CommandHistory> GetCommandHistory()
        {
            var history = new List<CommandHistory>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM CommandHistory ORDER BY Timestamp DESC";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                history.Add(new CommandHistory
                {
                    Id = reader.GetInt32(0),
                    Command = reader.GetString(1),
                    Response = reader.GetString(2),
                    Timestamp = reader.GetDateTime(3)
                });
            }
            return history;
        }
    }
}