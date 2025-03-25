using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
//Where the connection to the database will be established


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
        private int index;
        public Database(Leaderboard leaderboard) {
             connectionString = "Host=192.168.1.163;Username=user07;Password=d4T4b4S37;Database=AlienAttack";       //Local
            //***REMOVED***    //Global
            this.leaderboard = leaderboard;
            index = 0;
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

            //Not done async as we need the results from this one
        public bool checkIfUserExists(string username) {
            Debug.WriteLine("Starting check");
             using var command = dataSource.CreateCommand("SELECT username FROM Players WHERE username = '" + username + "'");
             using var reader =  command.ExecuteReader();
                //Code inside the while loop only runs when data is found so there must be a username corresponding to this
            while ( reader.Read())
            {
                Debug.WriteLine("Username must exist as it goes into this loop");
                return true;
            }
            Debug.WriteLine("Username doesn't exist");
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
            //Gets all of the data
            leaderboard.clearFileAndWriteHeaders();
            while (await reader.ReadAsync())
            {
                using var lineReader = reader.GetData(0);
                    //Prints all of the data
                while (lineReader.Read()) {
                    username = lineReader.GetFieldValue<string>(0);
                    highestLevel = lineReader.GetFieldValue<int>(1);
                    longestSurvived = lineReader.GetFieldValue<TimeSpan>(2).ToString();
                    highScore = lineReader.GetFieldValue<int>(3);
                    Debug.WriteLine(username+highestLevel+longestSurvived+highScore);
                    leaderboard.writeDataToPlayerFile(username, highestLevel, longestSurvived, highScore);
                }
            }
        }

        public void getGameData() { 
        
        }

        public void getGameData(string username) { 
        
        }

            //Only here for demo purposes:  Remove later
        public async Task testSelection() {
                //Runs SELECT command
            await using var command = dataSource.CreateCommand("SELECT (userid, username, highestlevel, longestsurvived) FROM Players ");
            await using var reader = await command.ExecuteReaderAsync();
                //Gets all of the data
            while (await reader.ReadAsync())
            {
                using var lineReader = reader.GetData(0);
                    //Prints all of the data
                while (lineReader.Read()) {
                    Debug.WriteLine("id: " + lineReader.GetFieldValue<int>(0));
                    Debug.WriteLine("username: " + lineReader.GetFieldValue<string>(1));
                    Debug.WriteLine("highestLevel: " + lineReader.GetFieldValue<int>(2));
                    Debug.WriteLine("longestSurvived: " + lineReader.GetFieldValue<TimeSpan>(3).ToString());
                }
            }
        }
    }
}
