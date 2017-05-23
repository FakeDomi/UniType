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
            this.previewLabel = new System.Windows.Forms.Label();
            this.modeLabel = new System.Windows.Forms.Label();
            this.inputLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // previewLabel
            // 
            this.previewLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.previewLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.previewLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.previewLabel.Location = new System.Drawing.Point(2, 2);
            this.previewLabel.Name = "previewLabel";
            this.previewLabel.Size = new System.Drawing.Size(80, 45);
            this.previewLabel.TabIndex = 0;
            this.previewLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.previewLabel.Paint += new System.Windows.Forms.PaintEventHandler(this.LabelsPaint);
            // 
            // modeLabel
            // 
            this.modeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.modeLabel.Location = new System.Drawing.Point(2, 46);
            this.modeLabel.Name = "modeLabel";
            this.modeLabel.Size = new System.Drawing.Size(22, 15);
            this.modeLabel.TabIndex = 1;
            this.modeLabel.Text = "U+";
            this.modeLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // inputLabel
            // 
            this.inputLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.inputLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.inputLabel.Location = new System.Drawing.Point(23, 46);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(59, 16);
            this.inputLabel.TabIndex = 1;
            this.inputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.inputLabel.Paint += new System.Windows.Forms.PaintEventHandler(this.LabelsPaint);
            // 
            // InputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(84, 64);
            this.ControlBox = false;
            this.Controls.Add(this.previewLabel);
            this.Controls.Add(this.inputLabel);
            this.Controls.Add(this.modeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label previewLabel;
        private System.Windows.Forms.Label modeLabel;
        private System.Windows.Forms.Label inputLabel;
    }
}

