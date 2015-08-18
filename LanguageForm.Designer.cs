namespace Aion_Launcher
{
    partial class LanguageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LanguageForm));
            this.langComboBox = new System.Windows.Forms.ComboBox();
            this.nextButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // langComboBox
            // 
            this.langComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.langComboBox.FormattingEnabled = true;
            this.langComboBox.Location = new System.Drawing.Point(15, 20);
            this.langComboBox.Name = "langComboBox";
            this.langComboBox.Size = new System.Drawing.Size(136, 21);
            this.langComboBox.TabIndex = 1;
            // 
            // nextButton
            // 
            this.nextButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.nextButton.Location = new System.Drawing.Point(100, 49);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(51, 23);
            this.nextButton.TabIndex = 2;
            this.nextButton.Text = "Next";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // LanguageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(163, 84);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.langComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LanguageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Launcher Language";
            this.Load += new System.EventHandler(this.Language_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox langComboBox;
        private System.Windows.Forms.Button nextButton;
    }
}