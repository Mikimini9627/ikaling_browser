using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Management;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;

namespace GesoTownBrowser
{
    /// <summary>
    /// MainFormクラス
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 画面ロード時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MainForm_Load(object sender, EventArgs e)
        {
            // 設定ファイルが存在しない場合はセットアップを実行する
            if (File.Exists(".\\config.json") == false)
            {
                MessageBox.Show("設定ファイルが存在しませんでした。\n初回設定を行います。", "ゲソタウンブラウザー", MessageBoxButtons.OK);

                try
                {
                    // ファイルダウンロード
                    await DownloadAsync("https://raw.githubusercontent.com/frozenpandaman/s3s/master/iksm.py", ".\\iksm.py");
                    await DownloadAsync("https://raw.githubusercontent.com/frozenpandaman/s3s/master/utils.py", ".\\utils.py");

                    // s3s.py生成
                    File.WriteAllText(Path.GetFullPath(".\\s3s.py"), "F_GEN_URL = \"https://api.imink.app/f\"" + Environment.NewLine + "A_VERSION = \"0.5.7\"");

                    // プロセス情報設定
                    var psInfo = new ProcessStartInfo
                    {
                        FileName = "pip",
                        Arguments = "install -r requirements.txt",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        WorkingDirectory = ".\\",
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true
                    };

                    using (Process? pProc = Process.Start(psInfo))
                    {
                        pProc?.WaitForExit();
                    }

                    // プロセス情報設定
                    psInfo = new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = "/c " + ".\\スプラトゥーン3_GTOKEN生成.py",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        WorkingDirectory = ".\\",
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true
                    };

                    // プロセス実行
                    using (Process? pProc = Process.Start(psInfo))
                    using (var swIn = pProc?.StandardInput)
                    using (var swOut = pProc?.StandardOutput)
                    {
                        try
                        {
                            string url = string.Empty;
                            while (true)
                            {
                                // 標準入力から1行読み込む
                                var strOut = swOut?.ReadLine();

                                // URLの場合
                                if (strOut?.Contains("https://accounts.nintendo.com") == true)
                                {
                                    // ユーザーにURLからリンクを取得させる
                                    InputForm inputForm = new(strOut);
                                    if (inputForm.ShowDialog() != DialogResult.OK)
                                    {
                                        Close();
                                        return;
                                    }

                                    // 標準入力へ渡す
                                    swIn?.WriteLine(inputForm.Result);
                                }

                                // 異常終了の場合
                                if (strOut == "1")
                                {
                                    MessageBox.Show("初回設定に失敗しました。\nアプリケーションを終了します。", "イカリング3", MessageBoxButtons.OK);
                                    Close();
                                    return;
                                }

                                // 正常終了の場合
                                if (strOut == "0")
                                {
                                    break;
                                }
                            }
                        }
                        finally
                        {
                            // 強制的に終わらせる
                            if (pProc != null)
                            {
                                KillProcessAndChildren(pProc.Id);
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("初回設定に失敗しました。\nアプリケーションを終了します。", "イカリング3", MessageBoxButtons.OK);
                    Close();
                    return;
                }
            }
            else
            {
                // Python実行してURLを取得する
                var python_result = PythonCall(".\\スプラトゥーン3_GTOKEN生成.py");

                // 結果表示
                if (python_result == null || python_result.Count() <= 0)
                {
                    MessageBox.Show("初回設定に失敗しました。\nアプリケーションを終了します。", "イカリング3", MessageBoxButtons.OK);
                    Close();
                    return;
                }

                if (python_result[0] != "0")
                {
                    MessageBox.Show("初回設定に失敗しました。\nアプリケーションを終了します。", "イカリング3", MessageBoxButtons.OK);
                    Close();
                    return;
                }
            }

            // WebView2初期化完了イベント追加
            wvView.CoreWebView2InitializationCompleted += WebView2InitializationCompleted;

            // WebView2初期化完了確認
            wvView?.EnsureCoreWebView2Async(null);
        }

        /// <summary>
        /// Coreの初期化完了後のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess == false)
            {
                return;
            }

            // 画面を最大化
            WindowState = FormWindowState.Maximized;

            // 設定ファイル読み込み
            Config? info = JsonConvert.DeserializeObject<Config>(File.ReadAllText(".\\config.json"));

            //Cookieの更新
            var newCookie = wvView.CoreWebView2.CookieManager.CreateCookie("_gtoken", info?.GTOKEN, "api.lp1.av5ja.srv.nintendo.net", "/");
            newCookie.IsSecure = true;
            wvView.CoreWebView2.CookieManager.AddOrUpdateCookie(newCookie);

            // イカリングへアクセス
            wvView.CoreWebView2.Navigate("https://api.lp1.av5ja.srv.nintendo.net/");
        }

        /// <summary>
        /// 子プロセス含めてキルする
        /// </summary>
        /// <param name="pid"></param>
        private void KillProcessAndChildren(int pid)
        {
            // Cannot close 'system idle process'.
            if (pid == 0)
            {
                return;
            }

            ManagementObjectSearcher searcher = new("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc.Cast<ManagementObject>())
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process proc = Process.GetProcessById(pid);
                proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
            catch (Win32Exception)
            {
                // Access denied
            }
        }

        /// <summary>
        /// ファイルをダウンロードする
        /// </summary>
        /// <param name="url"></param>
        /// <param name="file_path"></param>
        /// <returns></returns>
        private async Task DownloadAsync(string url, string file_path)
        {
            HttpClient httpClient = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var content = response.Content)
                    using (var stream = await content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream(file_path, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
            }
        }

        /// <summary>
        /// Python実行
        /// </summary>
        /// <param name="program"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private List<string> PythonCall(string program, string args = "")
        {
            ProcessStartInfo psInfo = new ProcessStartInfo();

            // 実行するファイルをセット
            psInfo.FileName = "Python";

            //引数をセット
            psInfo.Arguments = string.Format("\"{0}\" {1}", program, args);

            // コンソール・ウィンドウを開かない
            psInfo.CreateNoWindow = true;

            // シェル機能を使用しない
            psInfo.UseShellExecute = false;

            // 標準出力をリダイレクトする
            psInfo.RedirectStandardOutput = true;

            // プロセスを開始
            Process? p = Process.Start(psInfo);

            // アプリのコンソール出力結果を全て受け取る
            List<string> result = new List<string>();
            while (true)
            {
                string? line = p?.StandardOutput.ReadLine();
                if (line == null)
                {
                    break;
                }

                result.Add(line);
            }

            return result;
        }
    }
}
