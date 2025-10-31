using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static Ex05.AppCore;

namespace Ex05
{
    public class GameForm : Form
    {
        private List<List<Button>> m_GuessButtons;
        private List<Button> m_BlackGameButtons;
        private List<List<Button>> m_ResultButtons;
        private int m_MaxNumberOfGuesses;
        private const int k_GuessButtonsPerRow = 4;
        private const int k_ArrowButtonSize = 15;
        private const int k_ResultButtonSize = 25;
        private const int k_Spacing = 10;
        private const int k_ButtonStartLocationX = 12;
        private const int k_ButtonStartLocationY = 36;
        private const int k_ButtonSize = 30;
        public event Action<Color[],int> OnGuessSubmit;
        private Color[] m_CurrentGuess = null;
        CurrentGuessResult m_GuessResult;
        private int m_CurrentGuessIndex = 0;

        private struct CurrentGuessResult
        {
            public AppCore.eGuessResult[] m_Results;
            public int m_CurrentRow;
        }

        public void SetGuessResult(AppCore.eGuessResult[] i_Results, int i_CurrentRow)
        {
            m_GuessResult.m_Results = i_Results;
            m_GuessResult.m_CurrentRow = i_CurrentRow;
            ChangeButtonColors(i_Results, i_CurrentRow);
        }

        public GameForm(int i_MaxNumberOfGuesses)
        {
            m_MaxNumberOfGuesses = i_MaxNumberOfGuesses;
            m_CurrentGuess = new Color[m_MaxNumberOfGuesses];
            initializeComponent();
            createButtonList();
            createResultButtons();
            this.FormClosing += GameForm_FormClosing;
        }

        private void GameForm_FormClosing(object i_Sender, FormClosingEventArgs i_E)
        {
            Application.Exit();
        }

        public int GuessButtonsPerRow
        {
            get
            {
                return k_GuessButtonsPerRow;
            }
        }

        private void initializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = calculateWindowSize();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Name = "GameForm";
            this.Text = "Bool Pgia";
            this.ResumeLayout(false);
        }

        private void createButtonList()
        {
            // $G$ DSN-001 (-2) A custom control for a single guess row should have been implemented to encapsulate its logic and components.
            Size buttonSize = new Size(k_ButtonSize, k_ButtonSize);
            this.m_GuessButtons = new List<List<Button>>();
            this.m_BlackGameButtons = new List<Button>();
            int buttonCurrX = 0;
            int buttonCurrY = 0;

            for (int j = 0; j < k_GuessButtonsPerRow; j++)
            {
                buttonCurrX = k_ButtonStartLocationX + (buttonSize.Width + k_Spacing) * j;
                buttonCurrY = k_ButtonStartLocationY;

                Button b = new Button();
                b.Size = buttonSize;
                b.Location = new Point(buttonCurrX, buttonCurrY);
                b.BackColor = Color.Black;

                m_BlackGameButtons.Add(b);
                this.Controls.Add(b);
            }

            for (int i = 0; i < m_MaxNumberOfGuesses; i++)
            {
                buttonCurrY = k_ButtonStartLocationY + (i + 1) * (buttonSize.Height + k_Spacing);
                List<Button> row = new List<Button>();
                for (int j = 0; j < k_GuessButtonsPerRow; j++)
                {
                    buttonCurrX = k_ButtonStartLocationX + (j) * (buttonSize.Width + k_Spacing);
                    Button b = new Button();
                    b.Size = buttonSize;
                    b.Location = new Point(buttonCurrX, buttonCurrY);
                    b.BackColor = Color.White;
                    b.Click += new EventHandler(GuessButtons_OnClick);
                    row.Add(b);
                    this.Controls.Add(b);
                }

                this.m_GuessButtons.Add(row);
            }
        }

        private void updateArrowButtonState()
        {
            if (m_CurrentGuessIndex < m_ResultButtons.Count)
            {
                m_ResultButtons[m_CurrentGuessIndex][0].Enabled = isAllButtonsInTheCurrentRowColored();
            }
        }

        private void GuessButtons_OnClick(object i_Sender, EventArgs i_E)
        {
            Button clickedButton = i_Sender as Button;
            if (clickedButton != null)
            {
                if (isButtonInTheCurrentRow(clickedButton))
                {
                    Point screenLocation = clickedButton.PointToScreen(Point.Empty);
                    // $G$ NTT-999 (-7) There's no need to re-instantiate the "Color Picker" form instance each time it is used.
                    ColorPickerForm colorPicker = new ColorPickerForm(screenLocation, AppCore.GetColors());
                    colorPicker.ShowDialog();
                    clickedButton.BackColor = colorPicker.SelectedColor;
                    colorPicker.Close();
                    updateArrowButtonState();
                }
                else
                {
                    MessageBox.Show("You can only change colors in the current row.");
                }
            }
        }

        public void RevealResult(eGameColor[] i_Secret)
        {
            for(int i = 0; i < i_Secret.Length; i++)
            {
                m_BlackGameButtons[i].BackColor = AppCore.GetColors()[(int)i_Secret[i]];
            }
        }

        private void createResultButtons()
        {
            m_ResultButtons = new List<List<Button>>();
            int arrowLocationX = k_ButtonStartLocationX + k_GuessButtonsPerRow * (k_ButtonSize + k_Spacing);
            for (int i = 0; i < m_MaxNumberOfGuesses; i++)
            {
                int arrowLocationY = k_ButtonStartLocationY + (i + 1) * (k_ButtonSize + k_Spacing);

                Button arrowButton = new Button();
                arrowButton.Size = new Size(k_ArrowButtonSize, k_ArrowButtonSize);
                arrowButton.Text = "→";
                arrowButton.Location = new Point(arrowLocationX, arrowLocationY);
                arrowButton.Enabled = false;
                arrowButton.Click += new EventHandler(ArrowButtons_OnClick);
                List<Button> row = new List<Button>();
                row.Add(arrowButton);
                this.Controls.Add(arrowButton);
                for (int j = 0; j < k_GuessButtonsPerRow; j++)
                {
                    Button button = new Button();
                    button.Size = new Size(k_ResultButtonSize, k_ResultButtonSize);
                    int currButtonX = arrowLocationX + (j * (button.Width + k_Spacing)) + arrowButton.Width;
                    int currButtonY = arrowLocationY + k_ButtonSize / 4;
                    button.Location = new Point(currButtonX, currButtonY);
                    this.Controls.Add(button);
                    row.Add(button);
                }

                m_ResultButtons.Add(row);
            }
        }
        private void ArrowButtons_OnClick(object i_Sender, EventArgs i_E)
        {
            if (i_Sender is Button arrowButton)
            {
                int arrowRowIndex = -1;
                for (int i = 0; i < m_ResultButtons.Count; i++)
                {
                    if (m_ResultButtons[i].Contains(arrowButton))
                    {
                        arrowRowIndex = i;
                        break;
                    }
                }

                if (arrowRowIndex != m_CurrentGuessIndex)
                {
                    MessageBox.Show("You can only submit the current guess.");
                    return;
                }

                if (!isAllButtonsInTheCurrentRowColored())
                {
                    MessageBox.Show("You must color all buttons in the current row before submitting.");
                    return;
                }

                setCurrentGuess(arrowRowIndex);
                if (OnGuessSubmit != null)
                {
                    OnGuessSubmit(m_CurrentGuess, arrowRowIndex);
                }

                ChangeButtonColors(m_GuessResult.m_Results, m_GuessResult.m_CurrentRow);
                arrowButton.BackColor = Color.Green;
                m_CurrentGuessIndex++;
            }
        }

        private void setCurrentGuess(int i_RowIndex)
        {
            List<Button> row = m_GuessButtons[i_RowIndex];
            for (int i = 0; i < row.Count; i++)
            {
                m_CurrentGuess[i] = row[i].BackColor;
            }
        }

        private Size calculateWindowSize()
        {
            int guessButtonsWidth = k_GuessButtonsPerRow * k_ButtonSize + (k_GuessButtonsPerRow - 1) * k_Spacing;
            int resultButtonsWidth = k_GuessButtonsPerRow * k_ResultButtonSize + (k_GuessButtonsPerRow - 1) * k_Spacing;
            int totalWidth = k_ButtonStartLocationX + guessButtonsWidth + k_Spacing + k_ArrowButtonSize + k_Spacing + resultButtonsWidth + k_ButtonStartLocationX;
            int totalRows = m_MaxNumberOfGuesses + 1;
            int totalHeight = k_ButtonStartLocationY + totalRows * k_ButtonSize + (totalRows - 1) * k_Spacing + k_ButtonStartLocationY;

            return new Size(totalWidth, totalHeight);
        }

        public void ChangeButtonColors(AppCore.eGuessResult[] i_Results, int i_CurrentRow)
        {
            List<Button> row = m_ResultButtons[i_CurrentRow];
            int numOfBlacks = 0;
            int numOfWhites = 0;
            int numOfYellows = 0;
            int i;
            int j = 0;
            for (i = 0; i < i_Results.Length; i++)
            {
                switch(i_Results[i])
                {
                    case AppCore.eGuessResult.CorrectPosition:
                        numOfBlacks++;
                        break;
                    case AppCore.eGuessResult.Wrong:
                        numOfWhites++;
                        break;
                    case AppCore.eGuessResult.CorrectColor:
                        numOfYellows++;
                        break;
                }
            }
            
            for (i = 0; i < numOfBlacks; i++)
            {
                Button resultButton = row[j + 1];
                j++;
                resultButton.BackColor = Color.Black;
            }

            for (i = 0; i < numOfYellows; i++)
            {
                Button resultButton = row[j + 1];
                j++;
                resultButton.BackColor = Color.Yellow;
            }

            for (i = 0; i < numOfWhites; i++)
            {
                Button resultButton = row[j + 1];
                j++;
                resultButton.BackColor = Color.White;
            }
        }

        public void ShowGameOverMessage()
        {
            MessageBox.Show("Game Over! You've used all your guesses.");
        }

        public void ShowWinMessage()
        {
            MessageBox.Show("Congratulations! You've guessed the secret colors correctly!");
        }

        private bool isButtonInTheCurrentRow(Button i_ButtonPressed)
        {
            List<Button> currentRow = m_GuessButtons[m_CurrentGuessIndex];
            return currentRow.Contains(i_ButtonPressed);
            // $G$ CSS-999 (-3) Missing blank line before return statement.
        }

        private bool isAllButtonsInTheCurrentRowColored()
        {
            List<Button> currentRow = m_GuessButtons[m_CurrentGuessIndex];
            bool result = true;
            // $G$ CSS-999 (-2) bool template should be written like this - isXXX at the beginning.
            foreach (Button currentButton in currentRow)
            {
                if (currentButton.BackColor == Color.White)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }
}