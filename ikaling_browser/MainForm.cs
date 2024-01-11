using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Management;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;

namespace GesoTownBrowser
{
    /// <summary>
    /// MainForm�N���X
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ��ʃ��[�h���̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MainForm_Load(object sender, EventArgs e)
        {
            // �ݒ�t�@�C�������݂��Ȃ��ꍇ�̓Z�b�g�A�b�v�����s����
            if (File.Exists(".\\config.json") == false)
            {
                MessageBox.Show("�ݒ�t�@�C�������݂��܂���ł����B\n����ݒ���s���܂��B", "�Q�\�^�E���u���E�U�[", MessageBoxButtons.OK);

                try
                {
                    // �t�@�C���_�E�����[�h
                    await DownloadAsync("https://raw.githubusercontent.com/frozenpandaman/s3s/master/iksm.py", ".\\iksm.py");
                    await DownloadAsync("https://raw.githubusercontent.com/frozenpandaman/s3s/master/utils.py", ".\\utils.py");

                    // s3s.py����
                    File.WriteAllText(Path.GetFullPath(".\\s3s.py"), "F_GEN_URL = \"https://api.imink.app/f\"" + Environment.NewLine + "A_VERSION = \"0.5.7\"");

                    // �v���Z�X���ݒ�
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

                    // �v���Z�X���ݒ�
                    psInfo = new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = "/c " + ".\\�X�v���g�D�[��3_GTOKEN����.py",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        WorkingDirectory = ".\\",
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true
                    };

                    // �v���Z�X���s
                    using (Process? pProc = Process.Start(psInfo))
                    using (var swIn = pProc?.StandardInput)
                    using (var swOut = pProc?.StandardOutput)
                    {
                        try
                        {
                            string url = string.Empty;
                            while (true)
                            {
                                // �W�����͂���1�s�ǂݍ���
                                var strOut = swOut?.ReadLine();

                                // URL�̏ꍇ
                                if (strOut?.Contains("https://accounts.nintendo.com") == true)
                                {
                                    // ���[�U�[��URL���烊���N���擾������
                                    InputForm inputForm = new(strOut);
                                    if (inputForm.ShowDialog() != DialogResult.OK)
                                    {
                                        Close();
                                        return;
                                    }

                                    // �W�����͂֓n��
                                    swIn?.WriteLine(inputForm.Result);
                                }

                                // �ُ�I���̏ꍇ
                                if (strOut == "1")
                                {
                                    MessageBox.Show("����ݒ�Ɏ��s���܂����B\n�A�v���P�[�V�������I�����܂��B", "�C�J�����O3", MessageBoxButtons.OK);
                                    Close();
                                    return;
                                }

                                // ����I���̏ꍇ
                                if (strOut == "0")
                                {
                                    break;
                                }
                            }
                        }
                        finally
                        {
                            // �����I�ɏI��点��
                            if (pProc != null)
                            {
                                KillProcessAndChildren(pProc.Id);
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("����ݒ�Ɏ��s���܂����B\n�A�v���P�[�V�������I�����܂��B", "�C�J�����O3", MessageBoxButtons.OK);
                    Close();
                    return;
                }
            }
            else
            {
                // Python���s����URL���擾����
                var python_result = PythonCall(".\\�X�v���g�D�[��3_GTOKEN����.py");

                // ���ʕ\��
                if (python_result == null || python_result.Count() <= 0)
                {
                    MessageBox.Show("����ݒ�Ɏ��s���܂����B\n�A�v���P�[�V�������I�����܂��B", "�C�J�����O3", MessageBoxButtons.OK);
                    Close();
                    return;
                }

                if (python_result[0] != "0")
                {
                    MessageBox.Show("����ݒ�Ɏ��s���܂����B\n�A�v���P�[�V�������I�����܂��B", "�C�J�����O3", MessageBoxButtons.OK);
                    Close();
                    return;
                }
            }

            // WebView2�����������C�x���g�ǉ�
            wvView.CoreWebView2InitializationCompleted += WebView2InitializationCompleted;

            // WebView2�����������m�F
            wvView?.EnsureCoreWebView2Async(null);
        }

        /// <summary>
        /// Core�̏�����������̃C�x���g
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess == false)
            {
                return;
            }

            // ��ʂ��ő剻
            WindowState = FormWindowState.Maximized;

            // �ݒ�t�@�C���ǂݍ���
            Config? info = JsonConvert.DeserializeObject<Config>(File.ReadAllText(".\\config.json"));

            //Cookie�̍X�V
            var newCookie = wvView.CoreWebView2.CookieManager.CreateCookie("_gtoken", info?.GTOKEN, "api.lp1.av5ja.srv.nintendo.net", "/");
            newCookie.IsSecure = true;
            wvView.CoreWebView2.CookieManager.AddOrUpdateCookie(newCookie);

            // �C�J�����O�փA�N�Z�X
            wvView.CoreWebView2.Navigate("https://api.lp1.av5ja.srv.nintendo.net/");
        }

        /// <summary>
        /// �q�v���Z�X�܂߂ăL������
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
        /// �t�@�C�����_�E�����[�h����
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
        /// Python���s
        /// </summary>
        /// <param name="program"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private List<string> PythonCall(string program, string args = "")
        {
            ProcessStartInfo psInfo = new ProcessStartInfo();

            // ���s����t�@�C�����Z�b�g
            psInfo.FileName = "Python";

            //�������Z�b�g
            psInfo.Arguments = string.Format("\"{0}\" {1}", program, args);

            // �R���\�[���E�E�B���h�E���J���Ȃ�
            psInfo.CreateNoWindow = true;

            // �V�F���@�\���g�p���Ȃ�
            psInfo.UseShellExecute = false;

            // �W���o�͂����_�C���N�g����
            psInfo.RedirectStandardOutput = true;

            // �v���Z�X���J�n
            Process? p = Process.Start(psInfo);

            // �A�v���̃R���\�[���o�͌��ʂ�S�Ď󂯎��
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
