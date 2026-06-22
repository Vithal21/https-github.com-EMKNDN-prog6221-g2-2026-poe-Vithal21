namespace CyberSecurityChatbot.Models
{
    public class QuizQuestion
    {
        public string Question { get; set; }

        public string Option1 { get; set; }

        public string Option2 { get; set; }

        public string Option3 { get; set; }

        public int CorrectAnswer { get; set; }

        public string Explanation { get; set; }
    }
}