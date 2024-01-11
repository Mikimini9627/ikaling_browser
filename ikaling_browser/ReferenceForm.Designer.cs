namespace GesoTownBrowser
{
    partial class ReferenceForm
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
            pbRef = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pbRef).BeginInit();
            SuspendLayout();
            // 
            // pbRef
            // 
            pbRef.Dock = DockStyle.Fill;
            pbRef.Image = Properties.Resources.Capture;
            pbRef.Location = new Point(0, 0);
            pbRef.Name = "pbRef";
            pbRef.Size = new Size(1053, 690);
            pbRef.TabIndex = 0;
            pbRef.TabStop = false;
            // 
            // ReferenceForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1053, 690);
            Controls.Add(pbRef);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ReferenceForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ニンテンドーオンライン APIトークン取得方法";
            ((System.ComponentModel.ISupportInitialize)pbRef).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pbRef;
    }
}