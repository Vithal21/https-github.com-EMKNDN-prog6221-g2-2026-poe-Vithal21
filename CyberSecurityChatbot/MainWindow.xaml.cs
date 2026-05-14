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

            // Clear textbox
            txtUserInput.Clear();
        }
    }
}