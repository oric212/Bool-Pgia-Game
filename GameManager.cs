using System.Drawing;
using System.Windows.Forms;

namespace Ex05
{
    public class GameManager
    {
        private GameLogic m_GameLogic;
        private GameForm m_GameForm;
        private StartForm m_StartForm;
        private const int k_MaxNumberOfGuesses = 10;
        private const int k_MinNumberOfGuesses = 4;
        private int m_selectedNumberOfGuesses;
        private bool m_IsWon = false;
        private bool m_IsGameStillRunning = false;

        public GameManager()
        {
            m_StartForm = new StartForm();
            m_StartForm.StartGameRequested += OnGameStart;
        }

        public void Run()
        {
            Application.Run(m_StartForm);
        }
        // $G$ CSS-999 (-3) Event handler method names should follow the convention: control name, underscore, and event name (e.g., buttonOK_Click).
        private void OnGameStart(string i_UserInput)
        {
            if (int.TryParse(i_UserInput, out int userChoice))
            {
                if (userChoice >= k_MinNumberOfGuesses && userChoice <= k_MaxNumberOfGuesses)
                {
                    m_selectedNumberOfGuesses = userChoice;
                    m_GameForm = new GameForm(m_selectedNumberOfGuesses);
                    m_GameLogic = new GameLogic(m_GameForm.GuessButtonsPerRow);
                    m_GameForm.OnGuessSubmit += OnGuessSubmit;
                    m_GameForm.Show();
                    m_IsGameStillRunning = true;
                    // $G$ DSN-999 (-2) Avoid using .Hide() - The configuration form should be instantiated and shown. Once it is closed, the game form should then be instantiated and shown.
                    m_StartForm.Hide();
                }
                else
                {
                    MessageBox.Show($"Please enter a number between {k_MinNumberOfGuesses} and {k_MaxNumberOfGuesses}.");
                }
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter a number.");
            }
        }

        private void OnGuessSubmit(Color[] i_RecivedGuessFromUi, int i_CurrentRow)
        {
            if (i_CurrentRow + 1 == m_selectedNumberOfGuesses)
            {
                m_IsGameStillRunning = false;
            }
            AppCore.eGameColor[] convertedGuess = AppCore.ConvertColorsToGameColors(i_RecivedGuessFromUi);
            AppCore.eGuessResult[] currentGuessOutcome = m_GameLogic.ProccessGuessResults(convertedGuess);
            m_GameForm.SetGuessResult(currentGuessOutcome, i_CurrentRow);
            m_IsWon = m_GameLogic.CompareGuess(convertedGuess);
            if (m_IsWon)
            {
                m_GameForm.RevealResult(m_GameLogic.Secret);
                m_GameForm.ShowWinMessage();
                askForNewGame();
            }
            else
            {
                if (!m_IsGameStillRunning)
                {
                    m_GameForm.RevealResult(m_GameLogic.Secret);
                    m_GameForm.ShowGameOverMessage();
                    askForNewGame();
                }
            }
        }
        // $G$ SFN-008 (-3) The game logic does not meet the requirements. Starting a new game should only be possible after the player exits the current game.
        private void askForNewGame()
        {
            DialogResult result = MessageBox.Show(
                "Would you like to play again?",
                "Play Again",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                m_GameForm.Hide();
                m_StartForm.Show();
            }
            else
            {
                m_GameForm.Close();
            }
        }
    }
}