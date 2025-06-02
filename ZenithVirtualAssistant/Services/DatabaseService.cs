using System;
using Microsoft.Data.Sqlite;
using ZenithVirtualAssistant.Models;

namespace ZenithVirtualAssistant.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString = "Data Source=zenith.db";

        public DatabaseService()
        {
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
                    Command TEXT NOT NULL,
                    Response TEXT,
                    Timestamp DATETIME NOT NULL
                )";
            command.ExecuteNonQuery();
        }

        public void LogCommand(string command, string response)
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
    }
}