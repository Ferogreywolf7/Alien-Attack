using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Where the connection to the database will be established


namespace Alien_Attack
{
    internal class Database
    {
        private string databaseConnection;

        public Database() { 
        databaseConnection = "Server=192.168.1.164;User ID=root;Password=F3rogreywolf#7";
		new MySqlConnection(databaseConnection);
        }
    }
}
