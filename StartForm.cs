using System;
using System.Windows.Forms;

namespace Ex05
{
    public class StartForm : Form
    {
        private TextBox m_TextBox1;
        private Label m_Label1;
        private Button m_StartButton;
        public event Action<string> StartGameRequested;

        public StartForm()
        {
            initializeComponent();
            this.Font = new System.Drawing.Font("Arial", 16F);
            this.FormClosing += StartForm_FormClosing;  // Subscribe to FormClosing event
        }

        private void initializeComponent()
        {
            // $G$ NTT-999 (-3) Redundant namespace prefix. 
            this.m_StartButton = new System.Windows.Forms.Button();
            this.m_TextBox1 = new System.Windows.Forms.TextBox();
            this.m_Label1 = new System.Windows.Forms.Label();
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen; // Center the form on the screen
            this.SuspendLayout();
            // 
            // btnGuessCount
            // 
            this.m_StartButton.Location = new System.Drawing.Point(166, 258);
            this.m_StartButton.Name = "btnGuessCount";
            this.m_StartButton.Size = new System.Drawing.Size(205, 93);
            this.m_StartButton.TabIndex = 0;
            this.m_StartButton.Text = "Start";
            this.m_StartButton.UseVisualStyleBackColor = true;
            this.m_StartButton.Click += new System.EventHandler(OnStartButton_Click);
            // 
            // textBox1
            // 
            this.m_TextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.m_TextBox1.Location = new System.Drawing.Point(324, 93);
            this.m_TextBox1.Name = "m_TextBox1";
            this.m_TextBox1.Size = new System.Drawing.Size(100, 29);
            this.m_TextBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.m_Label1.AutoSize = true;
            this.m_Label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.m_Label1.Location = new System.Drawing.Point(83, 96);
            this.m_Label1.Name = "m_Label1";
            this.m_Label1.Size = new System.Drawing.Size(187, 25);
            this.m_Label1.TabIndex = 2;
            this.m_Label1.Text = "Number of chances:";
            // 
            // StartForm
            // 
            this.ClientSize = new System.Drawing.Size(554, 352);
            this.Controls.Add(this.m_Label1);
            this.Controls.Add(this.m_TextBox1);
            this.Controls.Add(this.m_StartButton);
            this.Name = "StartForm";
            this.Text = "Start Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void OnStartButton_Click(object i_Sender, EventArgs i_E)
        {
            StartGameRequested?.Invoke(m_TextBox1.Text);
        }

        private void StartForm_FormClosing(object i_Sender, FormClosingEventArgs i_E)
        {
            Application.Exit();  // Exit the entire application when this form is closing
        }
    }
}