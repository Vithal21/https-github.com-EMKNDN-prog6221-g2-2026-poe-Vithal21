using CyberSecurityChatbot.Classes;
using System.Media;
using System.Windows;

namespace CyberSecurityChatbot
{
    public partial class MainWindow : Window
    {
        Chatbot bot = new Chatbot();

        public MainWindow()
        {
            InitializeComponent();

            PlayGreeting();

            txtChat.AppendText("BOT: Welcome to the Cybersecurity Awareness Bot!\n\n");
        }

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

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string userInput = txtUserInput.Text;

            if (string.IsNullOrWhiteSpace(userInput))
            {
                MessageBox.Show("Please enter a message.");
                return;
            }

            txtChat.AppendText("YOU: " + userInput + "\n");

            string response = bot.GetResponse(userInput);

            txtChat.AppendText("BOT: " + response + "\n\n");

            txtUserInput.Clear();
        }
    }
}