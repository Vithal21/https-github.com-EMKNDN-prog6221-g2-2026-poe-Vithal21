using MySql.Data.MySqlClient;

namespace CyberSecurityChatbot.Data
{
    public class DatabaseManager
    {
        private readonly string connectionString =
            "server=localhost;database=CyberSecurityChatbotDB;uid=root;pwd=Ankisha@2026;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}