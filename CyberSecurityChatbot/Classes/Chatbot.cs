using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot.Classes
{
    /// <summary>
    /// Handles chatbot logic, keyword detection,
    /// memory, sentiment detection, and responses.
    /// </summary>
    public class Chatbot
    {
        // Random object for random responses
        Random random = new Random();

        // Stores user's favourite topic
        public string FavouriteTopic { get; set; }

        // Stores current topic for conversation flow
        private string currentTopic = "";

        /// <summary>
        /// Dictionary storing keywords and responses.
        /// </summary>
        Dictionary<string, List<string>> responses =
            new Dictionary<string, List<string>>()
        {
            {
                "password",
                new List<string>()
                {
                    "Use strong passwords with symbols and numbers.",
                    "Avoid using personal information in passwords.",
                    "Use different passwords for each account."
                }
            },

            {
                "phishing",
                new List<string>()
                {
                    "Avoid clicking suspicious links.",
                    "Check email addresses carefully.",
                    "Phishing scams often create urgency."
                }
            },

            {
                "privacy",
                new List<string>()
                {
                    "Review privacy settings regularly.",
                    "Enable two-factor authentication.",
                    "Avoid sharing personal information online."
                }
            },

            {
                "vpn",
                new List<string>()
                {
                    "VPNs help protect your online privacy.",
                    "A VPN encrypts your internet connection."
                }
            }
        };

        /// <summary>
        /// Processes user input and returns chatbot response.
        /// </summary>
        public string GetResponse(string input)
        {
            // Convert input to lowercase
            input = input.ToLower();

            #region Greeting Responses

            // Greeting responses
            if (input.Contains("hello") ||
                input.Contains("hi") ||
                input.Contains("hey"))
            {
                string[] greetings =
                {
                    "Hello! How can I help you stay safe online today?",
                    "Hi there! Welcome to the Cybersecurity Awareness Bot.",
                    "Hey! I'm here to help you with cybersecurity awareness.",
                    "Hello! Ask me anything about online safety."
                };

                int greetingIndex = random.Next(greetings.Length);

                return greetings[greetingIndex];
            }

            #endregion

            #region Sentiment Detection

            // SENTIMENT DETECTION
            if (input.Contains("worried"))
            {
                return "It's understandable to feel worried. Staying informed helps protect you online.";
            }

            if (input.Contains("frustrated"))
            {
                return "Cybersecurity can feel difficult sometimes, but you're learning valuable skills.";
            }

            #endregion

            #region Memory System

            // MEMORY SYSTEM
            if (input.Contains("i like"))
            {
                FavouriteTopic =
                    input.Replace("i like", "").Trim();

                return $"Great! I'll remember that you like {FavouriteTopic}.";
            }

            #endregion

            #region Conversation Flow

            // CONVERSATION FLOW
            if (input.Contains("tell me more") ||
                input.Contains("another tip"))
            {
                if (currentTopic != "")
                {
                    return GetRandomResponse(currentTopic);
                }
            }

            #endregion

            #region Keyword Recognition

            // KEYWORD RECOGNITION
            foreach (var keyword in responses.Keys)
            {
                if (input.Contains(keyword))
                {
                    currentTopic = keyword;

                    return GetRandomResponse(keyword);
                }
            }

            #endregion

            #region General Responses

            // GENERAL RESPONSES
            if (input.Contains("how are you"))
            {
                return "I'm doing great and ready to help keep you safe online!";
            }

            if (input.Contains("thank you"))
            {
                return "You're welcome! Stay cyber safe.";
            }

            if (input.Contains("bye"))
            {
                return "Goodbye! Stay safe online.";
            }

            #endregion

            // DEFAULT RESPONSE
            return "I didn't quite understand that. Please try again.";
        }

        /// <summary>
        /// Returns a random response for selected topic.
        /// </summary>
        private string GetRandomResponse(string keyword)
        { 
            // Get response list
            List<string> possibleResponses =
                responses[keyword];

            // Generate random index 
            int index =
                random.Next(possibleResponses.Count);

            // Return random response
            return possibleResponses[index];
        }
    }
}