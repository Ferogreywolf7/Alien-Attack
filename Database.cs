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
        private string userFound;
        private string userid;
        public Database() {
            connectionString = "Host=192.168.1.163;Username=Admin;Password=Tru3N4s7;Database=AlienAttack";
            
        }

        public void tryConnectToDatabase() {
            dataSource = NpgsqlDataSource.Create(connectionString);
            Debug.WriteLine("attempted to connect to the database");
        }

        public async Task addUser(string userName) {
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

        public string getUserID(string username) {
            using var command = dataSource.CreateCommand("SELECT userid FROM Players WHERE username = '"+username+"'");
            using var reader = command.ExecuteReader();
            while ( reader.Read()) {
                userid = reader.GetString(0);
                return userid;
            }
            return "-1";
        }

        public async Task insertGameValues(int userid, int score, string gameMode, string timesurvived, int level) {
            DateTime dateofplay = DateTime.Today;
            await using (var cmd = dataSource.CreateCommand("INSERT INTO Games (userid, score, gamemode, timesurvived, dateofplay, level) VALUES ("+userid+", "+score+", "+gameMode+", "+timesurvived+", "+dateofplay+", "+level+");"))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }

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
