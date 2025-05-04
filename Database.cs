using Npgsql;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Alien_Attack
{
    internal class Database
    {
        private NpgsqlDataSource dataSource;
        private string connectionString;
        private int userid;
        private string username;
        private int highestLevel;
        private string longestSurvived;
        private int highScore;
        private Leaderboard leaderboard;
            //var is used when the datatype doesnt need to be named and can just be implied based off of other areas
        public Database(Leaderboard leaderboard) {
             connectionString = "Host=192.168.1.163;Username=user07;Password=d4T4b4S37;Database=AlienAttack";
            this.leaderboard = leaderboard;
        }

        public bool tryConnectToDatabase() {
            dataSource = NpgsqlDataSource.Create(connectionString);
            try
            {
                var cmd = dataSource.CreateCommand("SELECT * FROM Players;");
                cmd.ExecuteReader();
                Debug.WriteLine("Connected to database");
                return true;
            }
            catch {
                //Can't create connection to database
                Debug.WriteLine("Can't connect to the database");
                dataSource.Dispose();
                return false;
            }
        }

        public async Task connectToDatabase() {
            dataSource = NpgsqlDataSource.Create(connectionString);
        }

        public async Task addUser(string userName) {
                //Runs SQL command to add a new user into the database
            await using (var cmd = dataSource.CreateCommand("INSERT INTO Players (username, highestlevel, longestsurvived, highscore) VALUES ('" + userName+"', 0, '00:00', 0);")) {
                await cmd.ExecuteNonQueryAsync();
            }
        }

            //Runs a select command and certain parts will run based on whether or not the user exists
        public bool checkIfUserExists(string username) {
             using var command = dataSource.CreateCommand("SELECT username FROM Players WHERE username = '" + username + "'");
             using var reader =  command.ExecuteReader();
            while ( reader.Read())
            {
                    //Code inside the while loop only runs when data is found so there must be a username corresponding to this
                return true;
            }
            return false;
        }

        public int getUserID(string username) {
                //Runs SQL command to get the userid for a specific username
            using var command = dataSource.CreateCommand("SELECT userid FROM Players WHERE username = '"+username+"'");
            using var reader = command.ExecuteReader();
                //gets the value for the string
            while ( reader.Read()) {
                userid = reader.GetInt32(0);
                return Convert.ToInt32(userid);
            }
            return -1;
        }

        public async Task insertGameValues(int userid, int score, string gameMode, string timesurvived, int level) {
                //Runs SQL command to insert a games data into the database
            DateTime dateofplay = DateTime.Now;
            await using (var cmd = dataSource.CreateCommand("INSERT INTO Games (userid, score, gamemode, timesurvived, dateofplay, level) VALUES ("+userid+", "+score+", '"+gameMode+"', '"+timesurvived+"', '"+dateofplay+"', "+level+");"))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }

        //Gets data and stores it in an encrypted file with the last modified date. When checking if the leaderboard has been updated, compare the date against when it was requested
        public async Task getLeaderboardData() { 
        await using var command = dataSource.CreateCommand("SELECT (username, highestlevel, longestsurvived, highScore) FROM Players;");
            await using var reader = await command.ExecuteReaderAsync();
            
            leaderboard.clearFileAndWriteHeaders();
            while (await reader.ReadAsync())
            {
                using var lineReader = reader.GetData(0);
                    //Gets all of the data
                while (lineReader.Read()) {
                    username = lineReader.GetFieldValue<string>(0);
                    highestLevel = lineReader.GetFieldValue<int>(1);
                    longestSurvived = lineReader.GetFieldValue<TimeSpan>(2).ToString();
                    highScore = lineReader.GetFieldValue<int>(3);
                    leaderboard.writeDataToPlayerFile(username, highestLevel, longestSurvived, highScore);
                    leaderboard.initialiseLeaderboard();
                }
            }
        }
    }
}