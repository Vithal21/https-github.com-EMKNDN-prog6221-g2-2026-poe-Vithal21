using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatbot.Classes
{
    public class TaskManager
    {
        private List<TaskItem> tasks =
            new List<TaskItem>();

        private int nextId = 1;

        public void AddTask(
            string title,
            string description)
        {
            TaskItem task = new TaskItem()
            {
                Id = nextId++,
                Title = title,
                Description = description,
                IsCompleted = false
            };

            tasks.Add(task);
        }

        public List<TaskItem> GetTasks()
        {
            return tasks;
        }

        public bool CompleteTask(int id)
        {
            TaskItem task =
                tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return false;
            }

            task.IsCompleted = true;

            return true;
        }

        public bool DeleteTask(int id)
        {
            TaskItem task =
                tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return false;
            }

            tasks.Remove(task);

            return true;
        }
    }
}