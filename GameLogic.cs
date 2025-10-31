using System;
using System.Linq;
using static Ex05.AppCore;

namespace Ex05
{
    public class GameLogic
    {
        // $G$ DSN-002 (-7) The logic was not supposed to adapt to the colors used in the UI.
        // The logic layer should not be aware of how the UI chooses to present the options to the user. 
        // For example, if we later want to create a different UI where the options are types of flowers or sounds, we would certainly want to reuse the exact same logic layer.
        private eGameColor[] m_Colors;
        private eGameColor[] m_Secret;
        public eGameColor[] Secret
        {
            get
            {
                return m_Secret;
            }
        }

        private int m_GuessButtonsPerRow;

        public GameLogic(int i_GuessButtonsPerRow)
        {
            m_GuessButtonsPerRow = i_GuessButtonsPerRow;
            m_Secret = new eGameColor[i_GuessButtonsPerRow];
            m_Colors = ConvertColorsToGameColors(AppCore.GetColors());
            generateSecret();
        }

        private void generateSecret()
        {
            Random rnd = new Random();
            for (int i = 0; i < m_GuessButtonsPerRow; i++)
            {
                bool isUnique = false;
                while(!isUnique)
                {
                    int randomNumber = rnd.Next(0, m_Colors.Length);
                    if (!m_Secret.Contains(m_Colors[randomNumber]))
                    {
                        m_Secret[i] = m_Colors[randomNumber];
                        isUnique = true;
                    }
                }
            }
        }

        public bool CompareGuess(eGameColor[] i_Guess)
        {
            bool isCorrect = true;
            for (int i = 0; i < m_GuessButtonsPerRow; i++)
            {
                if (i_Guess[i] != m_Secret[i])
                {
                    isCorrect = false;
                    break;
                }
            }

            return isCorrect;
        }

        public AppCore.eGuessResult[] ProccessGuessResults(eGameColor[] i_Guess)
        {
            AppCore.eGuessResult[] results = new AppCore.eGuessResult[m_GuessButtonsPerRow];
            for (int i = 0; i < m_GuessButtonsPerRow; i++)
            {
                if (i_Guess[i] == m_Secret[i])
                {
                    results[i] = AppCore.eGuessResult.CorrectPosition;
                }
                else if (m_Secret.Contains(i_Guess[i]))
                {
                    results[i] = AppCore.eGuessResult.CorrectColor;
                }
                else
                {
                    results[i] = AppCore.eGuessResult.Wrong;
                }
            }

            return results;
        }
    }
}
