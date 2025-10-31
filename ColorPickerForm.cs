using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Ex05
{
    public class ColorPickerForm : Form
    {
        List<List<Button>> m_ColorButtons;
        private const int k_ColorPickerFormNumberOfLines = 2;
        private const int k_ColorPickerFormButtonsPerLine = 4;
        private const int k_Spacing = 5;
        private const int k_ButtonSize = 30;
        private const int k_TotalButtonsWidth = (k_ColorPickerFormButtonsPerLine * k_ButtonSize) + (k_ColorPickerFormButtonsPerLine - 1) * k_Spacing;
        private const int k_TotalButtonsHeight = (k_ColorPickerFormNumberOfLines * k_ButtonSize) + (k_ColorPickerFormNumberOfLines - 1) * k_Spacing;
        private Size m_ButtonSize = new Size(30, 30);
        private Color[] m_Colors;

        public Color SelectedColor { get; private set; }

        public ColorPickerForm(Point i_FormLocation, Color[] i_Colors)
        {
            initializeComponent();
            this.StartPosition = FormStartPosition.Manual; // Set the start position to manual to allow custom location
            this.Location = i_FormLocation;
            m_Colors = i_Colors;
            createButtonList();
        }

        private void initializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(k_TotalButtonsWidth, k_TotalButtonsHeight);
            this.Text = "Color Picker";
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Set the form border style to fixed dialog
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.ResumeLayout(false);

        }

        private void createButtonList()
        {
            m_ColorButtons = new List<List<Button>>();
            int colorIdx = 0;
            for (int i = 0; i < k_ColorPickerFormNumberOfLines; i++)
            {
                List<Button> line = new List<Button>();
                for (int j = 0; j < k_ColorPickerFormButtonsPerLine; j++)
                {
                    Button b = new Button();
                    b.Size = m_ButtonSize;
                    b.Location = new Point(j * (m_ButtonSize.Width + k_Spacing), i * (m_ButtonSize.Height + k_Spacing));
                    b.BackColor = m_Colors[colorIdx];
                    line.Add(b);
                    this.Controls.Add(b);
                    b.Click += new EventHandler(ColorButtons_OnClick); // Correctly attach the event handler to the button's Click event  
                    colorIdx++;
                }
                m_ColorButtons.Add(line);
            }
        }

        private void ColorButtons_OnClick(object i_Sender, EventArgs i_E)
        {
            if (i_Sender is Button clickedButton)
            {
                SelectedColor = clickedButton.BackColor;
                this.Close();
            }
        }

        public void ColorPickerForm_FormClosed(object i_Sender, FormClosedEventArgs i_E)
        {
            this.Close(); // Close the form when it is closed
        }
    }
}