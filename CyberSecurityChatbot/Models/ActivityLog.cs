using System;

namespace CyberSecurityChatbot.Models
{
    public class ActivityLog
    {
        public int LogId { get; set; }

        public string ActionDescription { get; set; }

        public DateTime ActionDate { get; set; }
    }
}