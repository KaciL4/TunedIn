using System;
using System.Collections.Generic;
using System.Configuration; 
using System.Data;
using Microsoft.Data.SqlClient; 
using System.Globalization;
using System.IO;

namespace TunedIn
{
    public class DatabaseManager
    {
        // Read connection string from App.config
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["MusicLibraryDB"].ConnectionString;

        // This is the name of the database file in App.config
        private const string DatabaseName = "TunedInMusicDB";


        public DatabaseManager()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            // SQL Server requires a separate check to see if the database exists.

            //  Check if the database itself exists on LocalDB
            if (!DatabaseExists())
            {
                CreateDatabase();
            }

            // Create the Songs table (the database connection will succeed if the DB exists)
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = $@"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Songs')
                    CREATE TABLE Songs (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        FilePath NVARCHAR(450) UNIQUE NOT NULL, -- Use NVARCHAR for file paths
                        Title NVARCHAR(255),
                        Artist NVARCHAR(255),
                        Album NVARCHAR(255),
                        Duration NVARCHAR(10)
                    );";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Check if the database exists on the server
        private bool DatabaseExists()
        {
            // Use the Master database to check for the existence of your database
            string masterConnectionString = $"Server=(localdb)\\mssqllocaldb;Integrated Security=True;";
            string checkSql = $"SELECT count(*) FROM master.sys.databases WHERE name = '{DatabaseName}'";

            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(checkSql, connection))
                {
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

//Create the database
        private void CreateDatabase()
        {
            string masterConnectionString = $"Server=(localdb)\\mssqllocaldb;Integrated Security=True;";

            // NOTE: Cannot use parameterization with CREATE DATABASE
            string createSql = $"CREATE DATABASE {DatabaseName}";

            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(createSql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


        public void InsertSong(Song song)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // SQL is largely the same, but parameter placeholders use '@'
                string sql = @"
                    IF NOT EXISTS (SELECT 1 FROM Songs WHERE FilePath = @FilePath)
                    BEGIN
                        INSERT INTO Songs (FilePath, Title, Artist, Album, Duration)
                        VALUES (@FilePath, @Title, @Artist, @Album, @Duration);
                    END";

                using (var command = new SqlCommand(sql, connection))
                {
                    // Parameters
                    command.Parameters.AddWithValue("@FilePath", song.FilePath ?? string.Empty);
                    command.Parameters.AddWithValue("@Title", song.Title ?? string.Empty);
                    command.Parameters.AddWithValue("@Artist", song.Artist ?? string.Empty);
                    command.Parameters.AddWithValue("@Album", song.Album ?? string.Empty);
                    command.Parameters.AddWithValue("@Duration", song.Duration ?? string.Empty);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Song> LoadAllSongs()
        {
            var songs = new List<Song>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "SELECT Id, FilePath, Title, Artist, Album, Duration FROM Songs ORDER BY Title ASC;";

                using (var command = new SqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var song = new Song
                        {
                            Id = reader.GetInt32(0),
                            FilePath = reader.GetString(1),
                            // SQL Server reader uses GetString/GetInt32 reliably but requires checking for DBNull
                            Title = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Artist = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Album = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            Duration = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                        };

                        song.DurationTimeSpan = ParseDuration(song.Duration);
                        songs.Add(song);
                    }
                }
            }

            return songs;
        }

        private TimeSpan ParseDuration(string duration)
        {
            if (string.IsNullOrWhiteSpace(duration))
                return TimeSpan.Zero;

            if (TimeSpan.TryParse(duration, CultureInfo.InvariantCulture, out var ts))
                return ts;

            if (TimeSpan.TryParseExact(duration, "m\\:ss", CultureInfo.InvariantCulture, out var tsExact))
                return tsExact;

            return TimeSpan.Zero;
        }
    }
}