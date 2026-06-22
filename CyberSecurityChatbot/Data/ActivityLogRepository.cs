using CyberSecurityChatbot.Models;
using MySql.Data.MySqlClient;

namespace CyberSecurityChatbot.Data
{
    public class ActivityLogRepository
    {
        private readonly DatabaseManager db =
            new DatabaseManager();

        public void AddLog(string description)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();

                string query =
                    @"INSERT INTO ActivityLog
                    (ActionDescription, ActionDate)
                    VALUES
                    (@Description, @Date)";

                MySqlCommand cmd =
                    new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@Description",
                    description);

                cmd.Parameters.AddWithValue(
                    "@Date",
                    DateTime.Now);

                cmd.ExecuteNonQuery();
            }
        }

        public List<ActivityLog> GetLogs()
        {
            List<ActivityLog> logs =
                new List<ActivityLog>();

            using (var conn = db.GetConnection())
            {
                conn.Open();

                string query =
                    "SELECT * FROM ActivityLog ORDER BY ActionDate DESC";

                MySqlCommand cmd =
                    new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        logs.Add(new ActivityLog
                        {
                            LogId =
                                reader.GetInt32("LogId"),

                            ActionDescription =
                                reader.GetString("ActionDescription"),

                            ActionDate =
                                reader.GetDateTime("ActionDate")
                        });
                    }
                }
            }

            return logs;
        }
    }
}