using System.Collections.Generic;

namespace CyberSecurityChatbot.Classes
{
    public class ActivityLog
    {
        private List<string> activities =
            new List<string>();

        public void AddLog(string message)
        {
            activities.Add(message);
        }

        public List<string> GetLogs()
        {
            return activities;
        }
    }
}