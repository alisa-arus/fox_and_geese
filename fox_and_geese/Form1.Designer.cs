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
            this.btn_2_0 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_2_0
            // 
            this.btn_2_0.BackColor = System.Drawing.Color.White;
            this.btn_2_0.Location = new System.Drawing.Point(196, 140);
            this.btn_2_0.Name = "btn_2_0";
            this.btn_2_0.Size = new System.Drawing.Size(25, 25);
            this.btn_2_0.TabIndex = 0;
            this.btn_2_0.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.BackgroundImage = global::fox_and_geese.Properties.Resources.field;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(584, 781);
            this.Controls.Add(this.btn_2_0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 820);
            this.MinimumSize = new System.Drawing.Size(600, 820);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Лиса и гуси";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_2_0;
    }
}

