namespace domi1819.UniType
{
    partial class InputForm
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.uiUnicodePlusLabel = new System.Windows.Forms.Label();
            this.uiInputTextBox = new System.Windows.Forms.TextBox();
            this.uiPreviewLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // uiUnicodePlusLabel
            // 
            this.uiUnicodePlusLabel.AutoSize = true;
            this.uiUnicodePlusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiUnicodePlusLabel.Location = new System.Drawing.Point(7, 8);
            this.uiUnicodePlusLabel.Name = "uiUnicodePlusLabel";
            this.uiUnicodePlusLabel.Size = new System.Drawing.Size(28, 18);
            this.uiUnicodePlusLabel.TabIndex = 0;
            this.uiUnicodePlusLabel.Text = "U+";
            // 
            // uiInputTextBox
            // 
            this.uiInputTextBox.Location = new System.Drawing.Point(32, 8);
            this.uiInputTextBox.Name = "uiInputTextBox";
            this.uiInputTextBox.Size = new System.Drawing.Size(65, 20);
            this.uiInputTextBox.TabIndex = 1;
            // 
            // uiPreviewLabel
            // 
            this.uiPreviewLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.uiPreviewLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uiPreviewLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiPreviewLabel.Location = new System.Drawing.Point(106, 2);
            this.uiPreviewLabel.Name = "uiPreviewLabel";
            this.uiPreviewLabel.Size = new System.Drawing.Size(49, 32);
            this.uiPreviewLabel.TabIndex = 0;
            this.uiPreviewLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(157, 36);
            this.ControlBox = false;
            this.Controls.Add(this.uiInputTextBox);
            this.Controls.Add(this.uiPreviewLabel);
            this.Controls.Add(this.uiUnicodePlusLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "InputForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label uiUnicodePlusLabel;
        private System.Windows.Forms.TextBox uiInputTextBox;
        private System.Windows.Forms.Label uiPreviewLabel;

    }
}

