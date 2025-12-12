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

                string sql = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Songs')
                    CREATE TABLE Songs (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        FilePath NVARCHAR(450) UNIQUE NOT NULL,
                        Title NVARCHAR(255),
                        Artist NVARCHAR(255),
                        Album NVARCHAR(255),
                        Duration NVARCHAR(10)
                    );

                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Playlists')
                    CREATE TABLE Playlists (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(255) NOT NULL UNIQUE
                    );

                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PlaylistSongs')
                    CREATE TABLE PlaylistSongs (
                        PlaylistId INT NOT NULL,
                        SongId INT NOT NULL,
                        Position INT NOT NULL,
                        CONSTRAINT PK_PlaylistSongs PRIMARY KEY (PlaylistId, Position),
                        CONSTRAINT FK_PlaylistSongs_Playlists FOREIGN KEY (PlaylistId)
                            REFERENCES Playlists(Id) ON DELETE CASCADE,
                        CONSTRAINT FK_PlaylistSongs_Songs FOREIGN KEY (SongId)
                            REFERENCES Songs(Id) ON DELETE CASCADE
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
                    // Get the count of databases with the specified name
                    int count = (int)command.ExecuteScalar(); // returns the first column of the first row
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

                string sql = @"
            IF NOT EXISTS (SELECT 1 FROM Songs WHERE FilePath = @FilePath)
            BEGIN
                INSERT INTO Songs (FilePath, Title, Artist, Album, Duration)
                VALUES (@FilePath, @Title, @Artist, @Album, @Duration);
            END;

            SELECT Id FROM Songs WHERE FilePath = @FilePath;
        ";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@FilePath", song.FilePath ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Title", song.Title ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Artist", song.Artist ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Album", song.Album ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Duration", song.Duration ?? string.Empty);

                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        song.Id = Convert.ToInt32(result);
                    }
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

            if (TimeSpan.TryParse(duration, CultureInfo.InvariantCulture, out var ts)) // Regional and Language settings
                return ts;

            if (TimeSpan.TryParseExact(duration, "m\\:ss", CultureInfo.InvariantCulture, out var tsExact))
                return tsExact;

            return TimeSpan.Zero;
        }

        public void SavePlaylist(Playlist playlist)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // 1. Upsert playlist row
                    if (playlist.Id == 0)
                    {
                        // Insert new playlist (or reuse existing one by name)
                        string insertSql = @"
                            IF NOT EXISTS (SELECT 1 FROM Playlists WHERE Name = @Name)
                            BEGIN
                                INSERT INTO Playlists (Name) VALUES (@Name);
                                SELECT CAST(SCOPE_IDENTITY() AS INT);
                            END
                            ELSE
                            BEGIN
                                SELECT Id FROM Playlists WHERE Name = @Name;
                            END";

                        using (var cmd = new SqlCommand(insertSql, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Name", playlist.Name);
                            var result = cmd.ExecuteScalar();
                            playlist.Id = Convert.ToInt32(result);
                        }
                    }
                    else
                    {
                        // Update name if it changed
                        string updateSql = "UPDATE Playlists SET Name = @Name WHERE Id = @Id;";
                        using (var cmd = new SqlCommand(updateSql, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Name", playlist.Name);
                            cmd.Parameters.AddWithValue("@Id", playlist.Id);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // 2. Clear existing songs for this playlist
                    string deleteSql = "DELETE FROM PlaylistSongs WHERE PlaylistId = @PlaylistId;";
                    using (var deleteCmd = new SqlCommand(deleteSql, connection, transaction))
                    {
                        deleteCmd.Parameters.AddWithValue("@PlaylistId", playlist.Id);
                        deleteCmd.ExecuteNonQuery();
                    }

                    // 3. Re-insert playlist songs with positions
                    string insertSongSql = @"
                        INSERT INTO PlaylistSongs (PlaylistId, SongId, Position)
                        VALUES (@PlaylistId, @SongId, @Position);";

                    using (var insertCmd = new SqlCommand(insertSongSql, connection, transaction))
                    {
                        insertCmd.Parameters.Add("@PlaylistId", System.Data.SqlDbType.Int);
                        insertCmd.Parameters.Add("@SongId", System.Data.SqlDbType.Int);
                        insertCmd.Parameters.Add("@Position", System.Data.SqlDbType.Int);

                        for (int i = 0; i < playlist.Songs.Count; i++)
                        {
                            var song = playlist.Songs[i];

                            // Make sure song has a valid Id (must be in Songs table)
                            if (song.Id <= 0)
                                continue;

                            insertCmd.Parameters["@PlaylistId"].Value = playlist.Id;
                            insertCmd.Parameters["@SongId"].Value = song.Id;
                            insertCmd.Parameters["@Position"].Value = i;
                            insertCmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
            }
        }

        public List<Playlist> LoadAllPlaylists(List<Song> musicLibrary)
        {
            var playlists = new List<Playlist>();

            // Map Song.Id → Song for quick lookup
            var songById = new Dictionary<int, Song>();
            foreach (var song in musicLibrary)
            {
                if (!songById.ContainsKey(song.Id))
                    songById[song.Id] = song;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = @"
            SELECT p.Id, p.Name, ps.SongId, ps.Position
            FROM Playlists p
            LEFT JOIN PlaylistSongs ps ON ps.PlaylistId = p.Id
            ORDER BY p.Name, ps.Position;";

                using (var command = new SqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    Playlist current = null;
                    int currentPlaylistId = -1;

                    while (reader.Read())
                    {
                        int playlistId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        int songId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);

                        // New playlist row
                        if (current == null || playlistId != currentPlaylistId)
                        {
                            current = new Playlist(name)
                            {
                                Id = playlistId
                            };
                            playlists.Add(current);
                            currentPlaylistId = playlistId;
                        }

                        // Add songs if there is one for this row
                        if (songId != 0 && songById.TryGetValue(songId, out var song))
                        {
                            current.Songs.Add(song);
                        }
                    }
                }
            }

            return playlists;
        }


    }
}