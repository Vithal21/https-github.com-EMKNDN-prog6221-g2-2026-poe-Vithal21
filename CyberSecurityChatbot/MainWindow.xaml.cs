using CyberSecurityChatbot.Classes;
using System;
using System.Media;
using System.Windows;
namespace CyberSecurityChatbot
{
    /// <summary>
    /// Main chatbot window for user interaction.
    /// Handles GUI functionality and chatbot communication.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Creates chatbot object
        Chatbot bot = new Chatbot();
        private TaskManager taskManager =
    new TaskManager();

        private QuizManager quizManager =
    new QuizManager();

        private ActivityLog activityLog =
    new ActivityLog();

        /// <summary>
        /// Constructor for MainWindow
        /// Initializes components and plays greeting.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Play startup greeting
            PlayGreeting();

            // Welcome message shown to user
            txtChat.AppendText(
                "BOT: Welcome to the Cybersecurity Awareness Bot!\n\n");
        }

        /// <summary>
        /// Plays welcome WAV audio file.
        /// </summary>
        private void PlayGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("C:\\Users\\vitha\\OneDrive\\DesktopPractiseArray2\\Desktop\\CyberSecurityChatbot\\CyberSecurityChatbot\\Audio\\cyberHello.wav");

                player.Load();

                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Handles Send button click event.
        /// Sends user message to chatbot.
        /// </summary>
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            // Store user input
            string userInput = txtUserInput.Text;

            // Validation for empty input
            if (string.IsNullOrWhiteSpace(userInput))
            {
                MessageBox.Show("Please enter a message.");
                return;
            }

            // Display user message
            txtChat.AppendText("YOU: " + userInput + "\n");

            // Get chatbot response
            string response = bot.GetResponse(userInput);

            // Display chatbot response
            txtChat.AppendText("BOT: " + response + "\n\n");
            activityLog.AddLog("User: " + userInput);
            activityLog.AddLog("Bot: " + response);
            // Clear textbox
            txtUserInput.Clear();
     
        }
        private void btnTasks_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Visibility = Visibility.Collapsed;
            QuizPanel.Visibility = Visibility.Collapsed;
            LogPanel.Visibility = Visibility.Collapsed;

            TaskPanel.Visibility = Visibility.Visible;
        }

        private void btnQuiz_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Visibility = Visibility.Collapsed;
            TaskPanel.Visibility = Visibility.Collapsed;
            LogPanel.Visibility = Visibility.Collapsed;

            QuizPanel.Visibility = Visibility.Visible;

            QuizQuestion question =
                quizManager.GetCurrentQuestion();

            if (question != null)
            {
                txtQuizQuestion.Text =
                    question.Question;
            }
        }

        private void btnLog_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Visibility = Visibility.Collapsed;
            TaskPanel.Visibility = Visibility.Collapsed;
            QuizPanel.Visibility = Visibility.Collapsed;

            LogPanel.Visibility = Visibility.Visible;

            lstLog.Items.Clear();

            foreach (string log in activityLog.GetLogs())
            {
                lstLog.Items.Add(log);
            }
        }
        private void btnChat_Click(object sender, RoutedEventArgs e)
        {
            TaskPanel.Visibility = Visibility.Collapsed;
            QuizPanel.Visibility = Visibility.Collapsed;
            LogPanel.Visibility = Visibility.Collapsed;

            ChatPanel.Visibility = Visibility.Visible;
        }
        private void btnAddTask_Click(
    object sender,
    RoutedEventArgs e)
        {
            string taskName =
                txtTaskName.Text.Trim();

            if (string.IsNullOrWhiteSpace(taskName))
            {
                MessageBox.Show(
                    "Please enter a task.");
                return;
            }

            taskManager.AddTask(
                taskName,
                "Cybersecurity Task");

            activityLog.AddLog(
    "Task Added: " + taskName);

            RefreshTaskList();

            txtTaskName.Clear();
        }
        private void RefreshTaskList()
        {
            lstTasks.Items.Clear();

            foreach (var task in taskManager.GetTasks())
            {
                lstTasks.Items.Add(
                    $"ID: {task.Id} | " +
                    $"{task.Title} | " +
                    $"Completed: {task.IsCompleted}");
            }
        }

        private void btnCompleteTask_Click(
    object sender,
    RoutedEventArgs e)
        {
            if (lstTasks.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Select a task first.");
                return;
            }

            int id =
                taskManager.GetTasks()
                           [lstTasks.SelectedIndex]
                           .Id;

            taskManager.CompleteTask(id);
            activityLog.AddLog(
    "Task Completed: " + id);

            RefreshTaskList();
        }

        private void btnDeleteTask_Click(
    object sender,
    RoutedEventArgs e)
        {
            if (lstTasks.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Select a task first.");
                return;
            }

            int id =
                taskManager.GetTasks()
                           [lstTasks.SelectedIndex]
                           .Id;

            taskManager.DeleteTask(id);
            activityLog.AddLog(
    "Task Deleted: " + id);

            RefreshTaskList();
        }
        private void btnTrue_Click(
    object sender,
    RoutedEventArgs e)
        {
            ProcessQuizAnswer("true");
        }

        private void btnFalse_Click(
            object sender,
            RoutedEventArgs e)
        {
            ProcessQuizAnswer("false");
        }

        private void ProcessQuizAnswer(
            string answer)
        {
            string result =
                quizManager.SubmitAnswer(answer);

            MessageBox.Show(result);

            if (quizManager.QuizFinished())
            {
                MessageBox.Show(
                    $"Quiz Complete!\nScore: {quizManager.GetScore()}/10");

                txtQuizQuestion.Text =
                    "Quiz Finished!";
            }
            else
            {
                txtQuizQuestion.Text =
                    quizManager
                    .GetCurrentQuestion()
                    .Question;
            }
        }
    }
}