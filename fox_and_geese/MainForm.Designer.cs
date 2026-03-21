using System.Drawing;
using System.Windows.Forms;

namespace fox_and_geese
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.BackColor = Color.Beige;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(584, 781);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ResumeLayout(false);
            this.Size = new Size(550, 700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Лиса и гуси";

            gamePanel = new Panel
            {
                Location = new Point(25, 20),
                Size = new Size(7 * CellSize + 10, 7 * CellSize + 10),
                BackColor = Color.BurlyWood
            };

            statusLabel = new Label
            {
                Location = new Point(25, 7 * CellSize + 30),
                Size = new Size(350, 30),
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = "Ход гусей 🦆"
            };

            geeseCountLabel = new Label
            {
                Location = new Point(25, 7 * CellSize + 60),
                Size = new Size(300, 25),
                Font = new Font("Arial", 10),
                Text = "Осталось гусей: 13",
                ForeColor = Color.DarkBlue
            };

            captureCountLabel = new Label
            {
                Location = new Point(25, 7 * CellSize + 85),
                Size = new Size(300, 25),
                Font = new Font("Arial", 10),
                Text = "Съедено гусей: 0",
                ForeColor = Color.DarkRed
            };

            newGameButton = new Button
            {
                Location = new Point(350, 7 * CellSize + 30),
                Size = new Size(100, 35),
                Text = "Новая игра",
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            newGameButton.Click += NewGameButton_Click;

            undoButton = new Button
            {
                Location = new Point(460, 7 * CellSize + 30),
                Size = new Size(80, 35),
                Text = "Отмена",
                BackColor = Color.LightCoral,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            undoButton.Click += UndoButton_Click;

            this.Controls.Add(gamePanel);
            this.Controls.Add(statusLabel);
            this.Controls.Add(geeseCountLabel);
            this.Controls.Add(captureCountLabel);
            this.Controls.Add(newGameButton);
            this.Controls.Add(undoButton);
        }

        #endregion
    }
}

