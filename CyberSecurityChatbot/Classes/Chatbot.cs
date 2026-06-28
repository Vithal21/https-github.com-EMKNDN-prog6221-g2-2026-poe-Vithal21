using MySql.Data.MySqlClient;
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
        Random random = new Random();

        private TaskManager taskManager =
            new TaskManager();

        private ActivityLog activityLog =
            new ActivityLog();

        private QuizManager quizManager =
            new QuizManager();

        private bool quizMode = false;

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

        public string GetResponse(string input)
        {
            input = input.ToLower();

            // QUIZ MODE
            if (quizMode)
            {
                string result =
                    quizManager.SubmitAnswer(input);

                if (quizManager.QuizFinished())
                {
                    quizMode = false;

                    int score =
                        quizManager.GetScore();

                    activityLog.AddLog(
                        "Quiz Completed");

                    return result +
                           "\n\nQuiz Complete!" +
                           $"\nFinal Score: {score}/10";
                }

                QuizQuestion nextQuestion =
                    quizManager.GetCurrentQuestion();

                return result +
                       "\n\nNext Question:\n" +
                       nextQuestion.Question;
            }

            // START QUIZ
            if (input.Contains("start quiz"))
            {
                quizMode = true;

                activityLog.AddLog(
                    "Quiz Started");

                QuizQuestion question =
                    quizManager.GetCurrentQuestion();

                return "Cybersecurity Quiz Started!\n\n"
                       + question.Question;
            }

            // ADD TASK
            if (input.StartsWith("add task "))
            {
                string title =
                    input.Replace("add task ", "");

                taskManager.AddTask(
                    title,
                    "Cybersecurity Task");

                activityLog.AddLog(
                    "Task Added: " + title);

                return $"Task added successfully: {title}";
            }

            // SHOW TASKS
            if (input.Contains("show tasks"))
            {
                var tasks =
                    taskManager.GetTasks();

                if (tasks.Count == 0)
                {
                    return "No tasks found.";
                }

                string result =
                    "TASK LIST:\n\n";

                foreach (var task in tasks)
                {
                    result +=
                        $"ID: {task.Id} | " +
                        $"{task.Title} | " +
                        $"Completed: {task.IsCompleted}\n";
                }

                return result;
            }

            // COMPLETE TASK
            if (input.StartsWith("complete task "))
            {
                string idText =
                    input.Replace(
                        "complete task ",
                        "");

                int id;

                if (int.TryParse(
                    idText,
                    out id))
                {
                    bool success =
                        taskManager.CompleteTask(id);

                    if (success)
                    {
                        activityLog.AddLog(
                            "Task Completed: " + id);

                        return "Task completed.";
                    }
                }

                return "Task not found.";
            }

            // DELETE TASK
            if (input.StartsWith("delete task "))
            {
                string idText =
                    input.Replace(
                        "delete task ",
                        "");

                int id;

                if (int.TryParse(
                    idText,
                    out id))
                {
                    bool success =
                        taskManager.DeleteTask(id);

                    if (success)
                    {
                        activityLog.AddLog(
                            "Task Deleted: " + id);

                        return "Task deleted.";
                    }
                }

                return "Task not found.";
            }

            // SHOW ACTIVITY LOG
            if (input.Contains("show log"))
            {
                var activities =
                    activityLog.GetLogs();

                if (activities.Count == 0)
                {
                    return "Activity log is empty.";
                }

                string result =
                    "ACTIVITY LOG:\n\n";

                foreach (string activity in activities)
                {
                    result += activity + "\n";
                }

                return result;
            }

            // GREETINGS
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

                int greetingIndex =
                    random.Next(greetings.Length);

                return greetings[greetingIndex];
            }

            // SENTIMENT DETECTION
            if (input.Contains("worried"))
            {
                return "It's understandable to feel worried. Staying informed helps protect you online.";
            }

            if (input.Contains("frustrated"))
            {
                return "Cybersecurity can feel difficult sometimes, but you're learning valuable skills.";
            }

            // MEMORY SYSTEM
            if (input.Contains("i like"))
            {
                FavouriteTopic =
                    input.Replace(
                        "i like",
                        "").Trim();

                return $"Great! I'll remember that you like {FavouriteTopic}.";
            }

            // CONVERSATION FLOW
            if (input.Contains("tell me more") ||
                input.Contains("another tip"))
            {
                if (currentTopic != "")
                {
                    return GetRandomResponse(
                        currentTopic);
                }
            }

            // KEYWORD RECOGNITION
            foreach (var keyword in responses.Keys)
            {
                if (input.Contains(keyword))
                {
                    currentTopic = keyword;

                    return GetRandomResponse(
                        keyword);
                }
            }

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

            return "I didn't quite understand that. Please try again.";
        }

        private string GetRandomResponse(
            string keyword)
        {
            List<string> possibleResponses =
                responses[keyword];

            int index =
                random.Next(
                    possibleResponses.Count);

            return possibleResponses[index];
        }
    }
}