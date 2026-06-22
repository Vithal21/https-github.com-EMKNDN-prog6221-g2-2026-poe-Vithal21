using CyberSecurityChatbot.Classes;
using CyberSecurityChatbot.Data;
using CyberSecurityChatbot.Models;
using MySql.Data.MySqlClient;
using System;
using System.Media;
using System.Windows;
using System.Collections.Generic;


namespace CyberSecurityChatbot
{
    public partial class MainWindow : Window
    {
        private Chatbot bot = new Chatbot();

        private TaskRepository taskRepo =
            new TaskRepository();

        private ActivityLogRepository logRepo =
            new ActivityLogRepository();

        private List<QuizQuestion> quizQuestions =
    new List<QuizQuestion>();

        private int currentQuestionIndex = 0;

        private int score = 0;

        public MainWindow()
        {
            InitializeComponent();

            TestDatabaseConnection();

            LoadTasks();
            LoadQuizQuestions();
            PlayGreeting();

            txtChat.AppendText(
                "BOT: Welcome to the Cybersecurity Awareness Bot!\n\n");
        }

        /// <summary>
        /// Tests MySQL connection.
        /// </summary>
        private void TestDatabaseConnection()
        {
            try
            {
                DatabaseManager db =
                    new DatabaseManager();

                using (MySqlConnection conn =
                    db.GetConnection())
                {
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database Error:\n\n" + ex.Message,
                    "Connection Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Loads tasks from database.
        /// </summary>
        private void LoadTasks()
        {
            try
            {
                lstTasks.Items.Clear();

                var tasks =
                    taskRepo.GetTasks();

                foreach (var task in tasks)
                {
                    string status =
                        task.IsCompleted
                        ? "[DONE]"
                        : "[PENDING]";

                    lstTasks.Items.Add(
     $"{task.TaskId} - {status} - {task.Title} - Reminder: {task.ReminderDate:d}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Plays greeting sound.
        /// </summary>
        private void PlayGreeting()
        {
            try
            {
                SoundPlayer player =
                    new SoundPlayer(
                    @"C:\Users\vitha\OneDrive\DesktopPractiseArray2\Desktop\CyberSecurityChatbot\CyberSecurityChatbot\Audio\cyberHello.wav");

                player.Load();
                player.Play();
            }
            catch
            {
                // Ignore audio errors
            }
        }

        /// <summary>
        /// Chatbot send button.
        /// </summary>
        private void btnSend_Click(
            object sender,
            RoutedEventArgs e)
        {
            string userInput =
                txtUserInput.Text;

            if (string.IsNullOrWhiteSpace(userInput))
            {
                MessageBox.Show(
                    "Please enter a message.");
                return;
            }

            txtChat.AppendText(
                "YOU: " + userInput + "\n");

            string response =
                bot.GetResponse(userInput);

            txtChat.AppendText(
                "BOT: " + response + "\n\n");

            txtUserInput.Clear();
        }

        /// <summary>
        /// Add Task button.
        /// </summary>
        private void btnAddTask_Click(
     object sender,
     RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTaskTitle.Text))
                {
                    MessageBox.Show("Please enter a task title.");
                    return;
                }

                if (dpReminderDate.SelectedDate == null)
                {
                    MessageBox.Show("Please select a reminder date.");
                    return;
                }

                TaskItem task = new TaskItem()
                {
                    Title = txtTaskTitle.Text,
                    Description = txtTaskDescription.Text,
                    ReminderDate = dpReminderDate.SelectedDate,
                    IsCompleted = false
                };

                taskRepo.AddTask(task);

                logRepo.AddLog($"Task Added: {task.Title}");
                logRepo.AddLog($"Reminder Set: {task.ReminderDate:d}");

                lstActivityLog.Items.Add($"Task Added: {task.Title}");
                lstActivityLog.Items.Add($"Reminder Set: {task.ReminderDate:d}");

                LoadTasks();

                txtTaskTitle.Clear();
                txtTaskDescription.Clear();
                dpReminderDate.SelectedDate = null;

                MessageBox.Show("Task saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnCompleteTask_Click(
    object sender,
    RoutedEventArgs e)
        {
            try
            {
                if (lstTasks.SelectedItem == null)
                {
                    MessageBox.Show(
                        "Please select a task.");

                    return;
                }

                string selected =
                    lstTasks.SelectedItem.ToString();

                int taskId =
                    int.Parse(
                        selected.Split('-')[0].Trim());

                taskRepo.CompleteTask(taskId);

                logRepo.AddLog(
                    $"Task Completed: {taskId}");

                lstActivityLog.Items.Add(
                    $"Task Completed: {taskId}");

                LoadTasks();

                MessageBox.Show(
                    "Task marked complete.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Complete Task button.
        /// </summary>

        /// <summary>
        /// Delete Task button.
        /// </summary>
        private void btnDeleteTask_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                if (lstTasks.SelectedItem == null)
                {
                    MessageBox.Show(
                        "Please select a task.");

                    return;
                }

                string selected =
                    lstTasks.SelectedItem.ToString();

                int taskId =
                    int.Parse(
                        selected.Split('-')[0].Trim());

                taskRepo.DeleteTask(taskId);

                logRepo.AddLog(
                    $"Task Deleted: {taskId}");

                lstActivityLog.Items.Add(
                    $"Task Deleted: {taskId}");

                LoadTasks();

                MessageBox.Show(
                    "Task deleted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadQuizQuestions()
        {
            quizQuestions.Add(new QuizQuestion
            {
                Question = "What is phishing?",
                Option1 = "A scam email",
                Option2 = "A firewall",
                Option3 = "An antivirus",
                CorrectAnswer = 1,
                Explanation = "Phishing tricks users into revealing information."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "Strong passwords should contain:",
                Option1 = "Only names",
                Option2 = "Symbols and numbers",
                Option3 = "Only birthdays",
                CorrectAnswer = 2,
                Explanation = "Strong passwords include symbols, numbers and letters."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "A VPN helps by:",
                Option1 = "Encrypting internet traffic",
                Option2 = "Deleting viruses",
                Option3 = "Creating passwords",
                CorrectAnswer = 1,
                Explanation = "VPNs protect privacy by encrypting traffic."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "Should you click suspicious links?",
                Option1 = "Yes",
                Option2 = "Only once",
                Option3 = "No",
                CorrectAnswer = 3,
                Explanation = "Suspicious links may lead to scams or malware."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "Two-factor authentication improves:",
                Option1 = "Security",
                Option2 = "Internet speed",
                Option3 = "Storage space",
                CorrectAnswer = 1,
                Explanation = "2FA adds an extra layer of security."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "Social engineering attacks target:",
                Option1 = "People",
                Option2 = "Printers",
                Option3 = "Monitors",
                CorrectAnswer = 1,
                Explanation = "Attackers manipulate people into giving information."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "Public WiFi is:",
                Option1 = "Always secure",
                Option2 = "Potentially risky",
                Option3 = "Encrypted automatically",
                CorrectAnswer = 2,
                Explanation = "Public WiFi can expose your information."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "Antivirus software helps:",
                Option1 = "Detect malware",
                Option2 = "Write code",
                Option3 = "Increase RAM",
                CorrectAnswer = 1,
                Explanation = "Antivirus helps detect and remove threats."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "What should you do with software updates?",
                Option1 = "Ignore them",
                Option2 = "Install them",
                Option3 = "Delete them",
                CorrectAnswer = 2,
                Explanation = "Updates often fix security vulnerabilities."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "Sharing passwords is:",
                Option1 = "Safe",
                Option2 = "Recommended",
                Option3 = "Unsafe",
                CorrectAnswer = 3,
                Explanation = "Passwords should never be shared."
            });

            DisplayQuestion();
        }

        private void DisplayQuestion()
        {
            if (currentQuestionIndex >= quizQuestions.Count)
            {
                string message;

                if (score >= 8)
                    message = "Excellent cybersecurity knowledge!";
                else if (score >= 5)
                    message = "Good job, keep improving!";
                else
                    message = "You should spend more time learning cybersecurity.";

                MessageBox.Show(
                    $"Quiz Finished!\n\nScore: {score}/{quizQuestions.Count}\n\n{message}");

                return;
            }

            var q = quizQuestions[currentQuestionIndex];

            txtQuestion.Text = q.Question;

            rbOption1.Content = q.Option1;
            rbOption2.Content = q.Option2;
            rbOption3.Content = q.Option3;

            rbOption1.IsChecked = false;
            rbOption2.IsChecked = false;
            rbOption3.IsChecked = false;
        }

        private void btnSubmitQuiz_Click(object sender, RoutedEventArgs e)
        {
            int selectedAnswer = 0;

            if (rbOption1.IsChecked == true)
                selectedAnswer = 1;

            if (rbOption2.IsChecked == true)
                selectedAnswer = 2;

            if (rbOption3.IsChecked == true)
                selectedAnswer = 3;

            if (selectedAnswer == 0)
            {
                MessageBox.Show("Please select an answer.");
                return;
            }

            var question = quizQuestions[currentQuestionIndex];

            if (selectedAnswer == question.CorrectAnswer)
            {
                score++;

                MessageBox.Show(
                    "Correct!\n\n" +
                    question.Explanation);
            }
            else
            {
                MessageBox.Show(
                    "Incorrect.\n\n" +
                    question.Explanation);
                logRepo.AddLog(
    $"Quiz Completed - Score: {score}/{quizQuestions.Count}");

                lstActivityLog.Items.Add(
                    $"Quiz Completed - Score: {score}/{quizQuestions.Count}");
            }

            currentQuestionIndex++;

            DisplayQuestion();
        }
    }

}