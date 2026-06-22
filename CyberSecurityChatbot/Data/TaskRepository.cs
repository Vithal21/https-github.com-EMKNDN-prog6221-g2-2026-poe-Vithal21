using CyberSecurityChatbot.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace CyberSecurityChatbot.Data
{
    public class TaskRepository
    {
        private readonly DatabaseManager db = new DatabaseManager();

        public void AddTask(TaskItem task)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();

                string query =
                @"INSERT INTO Tasks
                (Title, Description, ReminderDate, IsCompleted)
                VALUES
                (@Title,@Description,@ReminderDate,@IsCompleted)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Title", task.Title);
                cmd.Parameters.AddWithValue("@Description", task.Description);
                cmd.Parameters.AddWithValue("@ReminderDate", task.ReminderDate);
                cmd.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);

                cmd.ExecuteNonQuery();
            }
        }

        public List<TaskItem> GetTasks()
        {
            List<TaskItem> tasks = new List<TaskItem>();

            using (var conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM Tasks";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new TaskItem
                        {
                            TaskId = reader.GetInt32("TaskId"),
                            Title = reader.GetString("Title"),
                            Description = reader.GetString("Description"),
                            ReminderDate = reader.IsDBNull(reader.GetOrdinal("ReminderDate"))
                                ? null
                                : reader.GetDateTime("ReminderDate"),
                            IsCompleted = reader.GetBoolean("IsCompleted")
                        });
                    }
                }
            }

            return tasks;
        }

        public void CompleteTask(int taskId)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();

                string query =
                    "UPDATE Tasks SET IsCompleted = 1 WHERE TaskId = @TaskId";

                MySqlCommand cmd =
                    new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@TaskId", taskId);

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteTask(int taskId)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();

                string query =
                    "DELETE FROM Tasks WHERE TaskId = @TaskId";

                MySqlCommand cmd =
                    new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@TaskId", taskId);

                cmd.ExecuteNonQuery();
            }
        }
    }
}