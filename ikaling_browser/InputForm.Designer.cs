namespace GesoTownBrowser
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
            this.lblDesc1 = new System.Windows.Forms.Label();
            this.edtUrl = new System.Windows.Forms.TextBox();
            this.lblDesc2 = new System.Windows.Forms.LinkLabel();
            this.lblDesc3 = new System.Windows.Forms.Label();
            this.edtApiToken = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDesc1
            // 
            this.lblDesc1.AutoSize = true;
            this.lblDesc1.Location = new System.Drawing.Point(12, 9);
            this.lblDesc1.Name = "lblDesc1";
            this.lblDesc1.Size = new System.Drawing.Size(371, 15);
            this.lblDesc1.TabIndex = 0;
            this.lblDesc1.Text = "1. 下記のURLにアクセスしニンテンドーオンライン APIトークンを取得してください。";
            // 
            // edtUrl
            // 
            this.edtUrl.Location = new System.Drawing.Point(12, 52);
            this.edtUrl.Multiline = true;
            this.edtUrl.Name = "edtUrl";
            this.edtUrl.ReadOnly = true;
            this.edtUrl.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.edtUrl.Size = new System.Drawing.Size(501, 117);
            this.edtUrl.TabIndex = 1;
            // 
            // lblDesc2
            // 
            this.lblDesc2.AutoSize = true;
            this.lblDesc2.LinkArea = new System.Windows.Forms.LinkArea(17, 3);
            this.lblDesc2.Location = new System.Drawing.Point(27, 28);
            this.lblDesc2.Name = "lblDesc2";
            this.lblDesc2.Size = new System.Drawing.Size(288, 21);
            this.lblDesc2.TabIndex = 2;
            this.lblDesc2.TabStop = true;
            this.lblDesc2.Text = "APIトークンの取得方法についてはこちらを参照してください。";
            this.lblDesc2.UseCompatibleTextRendering = true;
            this.lblDesc2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblDesc2_LinkClicked);
            // 
            // lblDesc3
            // 
            this.lblDesc3.AutoSize = true;
            this.lblDesc3.Location = new System.Drawing.Point(12, 185);
            this.lblDesc3.Name = "lblDesc3";
            this.lblDesc3.Size = new System.Drawing.Size(322, 15);
            this.lblDesc3.TabIndex = 3;
            this.lblDesc3.Text = "2. 取得したAPIトークンを下記に入力し、「決定」を押下してください。\r\n";
            // 
            // edtApiToken
            // 
            this.edtApiToken.Location = new System.Drawing.Point(12, 203);
            this.edtApiToken.Multiline = true;
            this.edtApiToken.Name = "edtApiToken";
            this.edtApiToken.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.edtApiToken.Size = new System.Drawing.Size(501, 117);
            this.edtApiToken.TabIndex = 4;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(438, 326);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "決定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // InputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 355);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.edtApiToken);
            this.Controls.Add(this.lblDesc3);
            this.Controls.Add(this.lblDesc2);
            this.Controls.Add(this.edtUrl);
            this.Controls.Add(this.lblDesc1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ニンテンドーオンライン APIトークン取得";
            this.Load += new System.EventHandler(this.InputForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblDesc1;
        private TextBox edtUrl;
        private LinkLabel lblDesc2;
        private Label lblDesc3;
        private TextBox edtApiToken;
        private Button btnOK;
    }
}