using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot.Classes
{
    public class Chatbot
    {
        Random random = new Random();

        public string UserName { get; set; }

        public string FavouriteTopic { get; set; }

        private string currentTopic = "";

        Dictionary<string, List<string>> responses =
            new Dictionary<string, List<string>>()
        {
            {
                "password",
                new List<string>()
                {
                    "Use strong passwords with symbols and numbers.",
                    "Avoid using your birthday in passwords.",
                    "Use a different password for every account."
                }
            },

            {
                "phishing",
                new List<string>()
                {
                    "Never click suspicious email links.",
                    "Check the sender's email carefully.",
                    "Phishing emails often create urgency."
                }
            },

            {
                "privacy",
                new List<string>()
                {
                    "Review your privacy settings regularly.",
                    "Avoid sharing personal information online.",
                    "Use two-factor authentication for better privacy."
                }
            }
        };

        public string GetResponse(string input)
        {
            input = input.ToLower();

            // SENTIMENT DETECTION
            if (input.Contains("worried"))
            {
                return "It's understandable to feel worried. Online scams are common, but staying informed helps protect you.";
            }

            if (input.Contains("frustrated"))
            {
                return "Cybersecurity can feel overwhelming sometimes, but you're doing great by learning about it.";
            }

            if (input.Contains("curious"))
            {
                return "Curiosity is great! Learning cybersecurity helps keep you safe online.";
            }

            // MEMORY
            if (input.Contains("i like"))
            {
                FavouriteTopic = input.Replace("i like", "").Trim();

                return $"Great! I'll remember that you're interested in {FavouriteTopic}.";
            }

            // FOLLOW-UP FLOW
            if (input.Contains("tell me more") || input.Contains("another tip"))
            {
                if (currentTopic != "")
                {
                    return GetRandomResponse(currentTopic);
                }
            }

            // KEYWORD RECOGNITION
            foreach (var keyword in responses.Keys)
            {
                if (input.Contains(keyword))
                {
                    currentTopic = keyword;

                    return GetRandomResponse(keyword);
                }
            }

            // GENERAL QUESTIONS
            if (input.Contains("how are you"))
            {
                return "I'm doing great and ready to help you stay safe online!";
            }

            if (input.Contains("your purpose"))
            {
                return "My purpose is to educate users about cybersecurity awareness.";
            }

            // DEFAULT RESPONSE
            return "I didn't quite understand that. Could you rephrase?";
        }

        private string GetRandomResponse(string keyword)
        {
            List<string> possibleResponses = responses[keyword];

            int index = random.Next(possibleResponses.Count);

            return possibleResponses[index];
        }
    }
}