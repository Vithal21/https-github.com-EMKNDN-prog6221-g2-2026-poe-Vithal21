using System.Collections.Generic;

namespace CyberSecurityChatbot.Classes
{
    public class QuizManager
    {
        private List<QuizQuestion> questions =
            new List<QuizQuestion>();

        private int currentQuestion = 0;

        private int score = 0;

        public QuizManager()
        {
            questions.Add(new QuizQuestion()
            {
                Question = "True or False: You should use the same password for all accounts.",
                Answer = "false",
                Explanation = "Different passwords reduce risk if one account is compromised."
            });

            questions.Add(new QuizQuestion()
            {
                Question = "True or False: Phishing emails often create urgency.",
                Answer = "true",
                Explanation = "Scammers use urgency to pressure victims."
            });

            questions.Add(new QuizQuestion()
            {
                Question = "True or False: Two-factor authentication improves security.",
                Answer = "true",
                Explanation = "2FA adds an extra layer of protection."
            });

            questions.Add(new QuizQuestion()
            {
                Question = "True or False: Public Wi-Fi is always safe.",
                Answer = "false",
                Explanation = "Public Wi-Fi can expose your data."
            });

            questions.Add(new QuizQuestion()
            {
                Question = "True or False: VPNs help protect privacy online.",
                Answer = "true",
                Explanation = "VPNs encrypt your connection."
            });

            questions.Add(new QuizQuestion()
            {
                Question = "True or False: Clicking unknown links is safe.",
                Answer = "false",
                Explanation = "Unknown links may lead to phishing websites."
            });

            questions.Add(new QuizQuestion()
            {
                Question = "True or False: Strong passwords should contain symbols.",
                Answer = "true",
                Explanation = "Symbols increase password complexity."
            });

            questions.Add(new QuizQuestion()
            {
                Question = "True or False: Software updates improve security.",
                Answer = "true",
                Explanation = "Updates often patch vulnerabilities."
            });

            questions.Add(new QuizQuestion()
            {
                Question = "True or False: Sharing personal information online is always safe.",
                Answer = "false",
                Explanation = "Oversharing increases security risks."
            });

            questions.Add(new QuizQuestion()
            {
                Question = "True or False: Antivirus software can help detect threats.",
                Answer = "true",
                Explanation = "Antivirus software helps identify malware."
            });
        }

        public QuizQuestion GetCurrentQuestion()
        {
            if (currentQuestion >= questions.Count)
            {
                return null;
            }

            return questions[currentQuestion];
        }

        public string SubmitAnswer(string answer)
        {
            QuizQuestion question =
                questions[currentQuestion];

            string result;

            if (answer.ToLower() ==
                question.Answer.ToLower())
            {
                score++;

                result =
                    "Correct! " +
                    question.Explanation;
            }
            else
            {
                result =
                    "Incorrect. " +
                    question.Explanation;
            }

            currentQuestion++;

            return result;
        }

        public int GetScore()
        {
            return score;
        }

        public bool QuizFinished()
        {
            return currentQuestion >= questions.Count;
        }
    }
}