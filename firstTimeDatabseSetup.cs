using Npgsql;
using System.Diagnostics;

namespace Alien_Attack
{
    internal class firstTimeDatabseSetup
    {
        private string connectionString;
        private NpgsqlDataSource dataSource;

        public firstTimeDatabseSetup(){
            connectionString = "Host=192.168.1.163;Username=Admin;Password=Tru3N4s7;Database=AlienAttack";
            dataSource = NpgsqlDataSource.Create(connectionString);
            try
            {
                var cmd = dataSource.CreateCommand("CREATE TABLE Players(UserID SERIAL PRIMARY KEY NOT NULL, username varChar NOT NULL, HighestLevel int, LongestSurvived time, highscore int);");
                cmd.ExecuteReader();
                cmd = dataSource.CreateCommand("CREATE TABLE Games( GameID SERIAL PRIMARY KEY NOT NULL, UserID int, score int NOT NULL, GameMode VarChar, TimeSurvived time, DateOfPlay date, score int, FOREIGN KEY (UserID) REFERENCES Players (UserID) );");
                cmd.ExecuteReader();
                Debug.WriteLine("Connected to database");
                
            }
            catch {
                Debug.WriteLine("Cant connect to database, need to rerun later");
                dataSource.Dispose();
            }
        }
    }
}
