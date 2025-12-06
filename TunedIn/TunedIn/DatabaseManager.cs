//// DatabaseManager.cs

//using System.Collections.Generic;
//using System.Data.SQLite;
//using System.IO;

//namespace TunedIn
//{
//    public class DatabaseManager
//    {
//        // Path to the SQLite database file
//        private const string DatabaseFile = "MusicLibrary.sqlite";
//        private readonly string _connectionString = $"Data Source={DatabaseFile};Version=3;";

//        public DatabaseManager()
//        {
//            InitializeDatabase();
//        }

//        private void InitializeDatabase()
//        {
//            // Only create the database file if it doesn't exist
//            if (!File.Exists(DatabaseFile))
//            {
//                SQLiteConnection.CreateFile(DatabaseFile);
//            }

//            using (var connection = new SQLiteConnection(_connectionString))
//            {
//                connection.Open();
//                string sql = @"
//                    CREATE TABLE IF NOT EXISTS Songs (
//                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
//                        FilePath TEXT UNIQUE NOT NULL,
//                        Title TEXT,
//                        Artist TEXT,
//                        Album TEXT,
//                        Duration TEXT
//                    );";
//                using (var command = new SQLiteCommand(sql, connection))
//                {
//                    command.ExecuteNonQuery();
//                }
//            }
//        }

//        // Saves a new Song entry to the database
//        public void InsertSong(Song song)
//        {
//            using (var connection = new SQLiteConnection(_connectionString))
//            {
//                connection.Open();
//                // Use INSERT OR IGNORE to prevent adding the same song file twice
//                string sql = @"
//                    INSERT OR IGNORE INTO Songs (FilePath, Title, Artist, Album, Duration) 
//                    VALUES (@FilePath, @Title, @Artist, @Album, @Duration)";

//                using (var command = new SQLiteCommand(sql, connection))
//                {
//                    command.Parameters.AddWithValue("@FilePath", song.FilePath);
//                    command.Parameters.AddWithValue("@Title", song.Title);
//                    command.Parameters.AddWithValue("@Artist", song.Artist);
//                    command.Parameters.AddWithValue("@Album", song.Album);
//                    command.Parameters.AddWithValue("@Duration", song.Duration);
//                    command.ExecuteNonQuery();
//                }
//            }
//        }

//        // Loads all songs from the database
//        public List<Song> LoadAllSongs()
//        {
//            var songs = new List<Song>();

//            using (var connection = new SQLiteConnection(_connectionString))
//            {
//                connection.Open();
//                string sql = "SELECT * FROM Songs";

//                using (var command = new SQLiteCommand(sql, connection))
//                using (var reader = command.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        var song = new Song
//                        {
//                            Id = reader.GetInt32(0),
//                            FilePath = reader.GetString(1),
//                            Title = reader.GetString(2),
//                            Artist = reader.GetString(3),
//                            Album = reader.GetString(4),
//                            Duration = reader.GetString(5)
//                            // Note: DurationTimeSpan will need to be calculated 
//                            // after loading, if needed for playback.
//                        };
//                        songs.Add(song);
//                    }
//                }
//            }
//            return songs;
//        }
//    }
//}