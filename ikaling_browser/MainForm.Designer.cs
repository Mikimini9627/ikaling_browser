namespace GesoTownBrowser
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            wvView = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)wvView).BeginInit();
            SuspendLayout();
            // 
            // wvView
            // 
            wvView.AllowExternalDrop = true;
            wvView.CreationProperties = null;
            wvView.DefaultBackgroundColor = Color.White;
            wvView.Dock = DockStyle.Fill;
            wvView.Location = new Point(0, 0);
            wvView.Name = "wvView";
            wvView.Size = new Size(800, 450);
            wvView.TabIndex = 0;
            wvView.ZoomFactor = 1D;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(wvView);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "イカリング3";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)wvView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 wvView;
    }
}
