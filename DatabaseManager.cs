using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;

namespace TunedIn
{
    public class DatabaseManager
    {
        // SQLite DB file name in your app folder
        private const string DatabaseFile = "MusicLibrary.db";
        private readonly string _connectionString = $"Data Source={DatabaseFile};Version=3;";


        public DatabaseManager()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            // Create the file if it doesn't exist (optional, SQLite will create it on connect too)
            if (!File.Exists(DatabaseFile))
            {
                using (File.Create(DatabaseFile))
                {
                    // Just create and close it
                }
            }

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string sql = @"
                    CREATE TABLE IF NOT EXISTS Songs (
                        Id       INTEGER PRIMARY KEY AUTOINCREMENT,
                        FilePath TEXT UNIQUE NOT NULL,
                        Title    TEXT,
                        Artist   TEXT,
                        Album    TEXT,
                        Duration TEXT
                    );";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertSong(Song song)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string sql = @"
                    INSERT OR IGNORE INTO Songs (FilePath, Title, Artist, Album, Duration)
                    VALUES (@FilePath, @Title, @Artist, @Album, @Duration);";

                using (var command = new SQLiteCommand(sql, connection))
                {
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

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string sql = "SELECT Id, FilePath, Title, Artist, Album, Duration FROM Songs;";

                using (var command = new SQLiteCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var song = new Song
                        {
                            Id = reader.GetInt32(0),
                            FilePath = reader.GetString(1),
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

            // Try normal parsing first
            if (TimeSpan.TryParse(duration, CultureInfo.InvariantCulture, out var ts))
                return ts;

            // Then try explicit "m:ss"
            if (TimeSpan.TryParseExact(duration, "m\\:ss", CultureInfo.InvariantCulture, out var tsExact))
                return tsExact;

            return TimeSpan.Zero;
        }
    }
}
