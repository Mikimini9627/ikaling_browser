namespace GesoTownBrowser
{
    /// <summary>
    /// 入力フォームクラス
    /// </summary>
    public partial class InputForm : Form
    {
        /// <summary>
        /// APIトークン
        /// </summary>
        internal string Result = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputForm(string strUrl)
        {
            InitializeComponent();

            // URL設定
            edtUrl.Text = strUrl;
        }

        /// <summary>
        /// ラベル押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDesc2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 参照画面表示
            ReferenceForm refForm = new();
            refForm.ShowDialog();
        }

        /// <summary>
        /// 決定ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            Result = edtApiToken.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// ロード時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputForm_Load(object sender, EventArgs e)
        {
            // 最前面に移動する
            BringToFront();
        }
    }
}
