using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SearchMyIndex
{
    public partial class Form1 : Form
    {
        private const string CONFKEY_COMPUTERNAME = "computername";
        private const string CONFKEY_DIR = "dir";
        private const string CONFKEY_SHARE_DIR = "shareDir";
        private const string CONFKEY_BACLUP_DIR = "backupDir";
        private const string CONFKEY_BACLUP_SHARE_DIR = "backupShareDir";

        private List<Dictionary<string, string>> confList = null;
        private List<string> fullNameList = null;

        private Dictionary<string, Dictionary<string, string>> resultConfDictionary = null;

        bool keyDownExecute = false;

        public Form1()
        {
            InitializeComponent();

            Config.LoadConfig();

            MakeMenu();

            LoadMyIndexes();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Location = Properties.Settings.Default.Form1Location;
            Size = Properties.Settings.Default.Form1Size;

            SearchIndex(txtSearchText.Text);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Form1Location = Location;
            Properties.Settings.Default.Form1Size = Size;
            Properties.Settings.Default.Save();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.PageUp)
            {
                if (ActiveControl == lstSearchResult && lstSearchResult.SelectedIndex <= 0)
                {
                    txtSearchText.Focus();
                }
            }

            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.PageDown)
            {
                if (ActiveControl == txtSearchText)
                {
                    lstSearchResult.Focus();
                    if (lstSearchResult.Items.Count > 0)
                    {
                        lstSearchResult.SelectedIndex = 0;
                    }
                }
            }

            if (ActiveControl != txtSearchText)
            {
                List<KeyManagerDelegate> addrs = KeyManager.GetKeyDown(e.KeyData);
                if (addrs != null)
                {
                    // キー処理でダイアログを開いた場合に、KeyDown実行中にKeyPressが発生し、SupressKeyPressが効かないため、
                    // KeyDown処理がある場合には自前のフラグでKeyPress処理を抑制している。
                    keyDownExecute = true;

                    foreach (KeyManagerDelegate addr in addrs)
                    {
                        addr(sender, e);
                    }

                    // KeyPressイベントを発生させないようにする
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!keyDownExecute)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    if (ActiveControl == txtSearchText)
                    {
                        btnSearch.PerformClick();
                    }

                    if (ActiveControl == lstSearchResult)
                    {
                        ExecItem();
                    }
                }

                if (ActiveControl != txtSearchText)
                {
                    List<KeyManagerDelegate> addrs = KeyManager.GetKeyPress(e.KeyChar);
                    if (addrs != null)
                    {
                        try
                        {
                            foreach (KeyManagerDelegate addr in addrs)
                            {
                                addr(sender, e);
                            }
                        }
                        // キー入力で設定を更新した場合にはループ中にaddrsが変更されてここに来る
                        catch (InvalidOperationException ex)
                        {
                            // 処理を打ち切る
                            return;
                        }
                    }
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            keyDownExecute = false;
        }

        private void txtSearchText_Enter(object sender, EventArgs e)
        {
            txtSearchText.SelectAll();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchIndex(txtSearchText.Text);
        }

        private void lstSearchResult_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ExecItem();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            ConfigForm configForm = new ConfigForm();
            configForm.ShowDialog();
        }

        private void btnRebuild_Click(object sender, EventArgs e)
        {
            Rebuild();
        }

        private void btnRebuildAll_Click(object sender, EventArgs e)
        {
            RebuildAll();
        }

        private void lstSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMgssage.Text = "";
        }

        private void lstSearchResult_Leave(object sender, EventArgs e)
        {
            lblMgssage.Text = "";
        }

        private void LoadMyIndexes()
        {
            confList = new List<Dictionary<string, string>>();
            fullNameList = new List<string>();

            resultConfDictionary = new Dictionary<string, Dictionary<string, string>>();

            string myIndexDIr = ReplaceCommandValue(Properties.Settings.Default.MyIndexDir);
            string[] indexes = Directory.GetFiles(myIndexDIr, "*.txt");

            foreach (string index in indexes)
            {
                LoadMyIndex(index);
            }
        }

        private void LoadMyIndex(string index)
        {
            Dictionary<string, string> conf = new Dictionary<string, string>();

            using (StreamReader sr = new StreamReader(index, Encoding.GetEncoding("shift_jis")))
            {
                while (sr.Peek() > -1)
                {
                    string lineStr = sr.ReadLine();

                    if (lineStr == "")
                    {
                        break;
                    }

                    string[] keyValue = lineStr.Split('=');
                    if (keyValue.Length != 2)
                    {
                        break;
                    }

                    string key = keyValue[0];
                    string value = keyValue[1];
                    conf[key] = value;
                }

                if (String.Compare(conf[CONFKEY_COMPUTERNAME], Environment.MachineName, true) == 0)
                {
                    // インデックスと同じPC

                    string dir = conf[CONFKEY_DIR];

                    while (sr.Peek() > -1)
                    {
                        string lineStr = sr.ReadLine();

                        if (lineStr != "")
                        {
                            confList.Add(conf);

                            string fullName = Path.Combine(dir, lineStr);
                            fullNameList.Add(fullName);
                        }
                    }
                }
                else
                {
                    // インデックスと異なるPC

                    string shareDir = conf[CONFKEY_SHARE_DIR];

                    while (sr.Peek() > -1)
                    {
                        string lineStr = sr.ReadLine();

                        if (lineStr != "")
                        {
                            confList.Add(conf);

                            string fullName = Path.Combine(shareDir, lineStr);
                            fullNameList.Add(fullName);
                        }
                    }
                }
            }
        }

        private void SearchIndex(string searchStr)
        {
            lstSearchResult.Items.Clear();
            resultConfDictionary.Clear();

            List<string> searchResultList = new List<string>();
            bool flagAll = (searchStr == "");
            string[] splitSearchStrs = searchStr.Split(' ');
            bool flagOR = false;

            if (splitSearchStrs.Length >= 2 && splitSearchStrs[1] == "|")
            {
                // OR
                if ((splitSearchStrs.Length % 2) == 0)
                {
                    MessageBox.Show(
                        "OR(|)を使用する場合には、a | b 、a | b | c 等の形式で入力して下さい。",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                for (int i = 0; i < splitSearchStrs.Length; i++)
                {
                    if ((i % 2) == 0 && splitSearchStrs[i].IndexOf('|') >= 0 ||
                        (i % 2) == 1 && splitSearchStrs[i] != "|")
                    {
                        MessageBox.Show(
                            "OR(|)を使用する場合には、a | b 、a | b | c 等の形式で入力して下さい。",
                            "エラー",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }

                flagOR = true;
            }
            else if (searchStr.IndexOf('|') >= 0)
            {
                MessageBox.Show(
                    "OR(|)を使用する場合には、a | b 、a | b | c 等の形式で入力して下さい。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < fullNameList.Count; i++)
            {
                string fullName = fullNameList[i];
                bool flagMatch = true;

                if (!flagAll)
                {
                    if (flagOR)
                    {
                        // OR
                        flagMatch = false;

                        for (int j = 0; j < splitSearchStrs.Length; j+=2)
                        {
                            if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(fullName, splitSearchStrs[j], CompareOptions.IgnoreCase | CompareOptions.IgnoreWidth | CompareOptions.IgnoreKanaType) >= 0)
                            {
                                flagMatch = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // AND
                        for (int j = 0; j < splitSearchStrs.Length; j++)
                        {
                            if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(fullName, splitSearchStrs[j], CompareOptions.IgnoreCase | CompareOptions.IgnoreWidth | CompareOptions.IgnoreKanaType) < 0)
                            {
                                flagMatch = false;
                                break;
                            }
                        }
                    }
                }

                if (flagMatch)
                {
                    searchResultList.Add(fullName);

                    Dictionary<string, string> conf = confList[i];
                    resultConfDictionary.Add(fullName, conf);
                }
            }

            searchResultList.Sort(new LogicalStringComparer());

            lstSearchResult.BeginUpdate();

            for (int i = 0; i < searchResultList.Count; i++)
            {
                lstSearchResult.Items.Add(searchResultList[i]);
            }

            lstSearchResult.EndUpdate();
        }

        private void ExecItem()
        {
            string fullName = (string)lstSearchResult.SelectedItem;

            if (Directory.Exists(fullName))
            {
                Dictionary<string, string> conf = resultConfDictionary[fullName];
                if (conf.ContainsKey(CONFKEY_BACLUP_DIR))
                {
                    string shareDir = conf[CONFKEY_SHARE_DIR];
                    string backupShareDir = conf[CONFKEY_BACLUP_SHARE_DIR];

                    string normalizationFullName = Path.GetFullPath(fullName);
                    string partName = null;
                    string backupFullName = null;
                    if (String.Compare(conf[CONFKEY_COMPUTERNAME], Environment.MachineName, true) == 0 || shareDir == null || backupShareDir == null)
                    {
                        string dir = conf[CONFKEY_DIR];
                        string backupDir = conf[CONFKEY_BACLUP_DIR];

                        partName = normalizationFullName.Substring(dir.Length + 1);
                        backupFullName = Path.Combine(backupDir, partName);
                    }
                    else
                    {
                        partName = normalizationFullName.Substring(shareDir.Length + 1);
                        backupFullName = Path.Combine(backupShareDir, partName);
                    }

                    if (Directory.Exists(backupFullName))
                    {
                        Process.Start(backupFullName);
                    }
                }

                Process.Start(fullName);
            }
            else if (File.Exists(fullName))
            {
                Process.Start(fullName);
            }
        }

        public void CopyFullName()
        {
            string fullName = (string)lstSearchResult.SelectedItem;
            if (fullName != null)
            {
                Clipboard.SetText(fullName);
                lblMgssage.Text = "クリップボードにフルパスファイル名をコピーしました：'" + fullName + "'";
            }
        }

        public void CutFile()
        {
            string fullName = (string)lstSearchResult.SelectedItem;
            if (fullName != null)
            {
                string[] filenames = { fullName };
                IDataObject data = new DataObject(DataFormats.FileDrop, filenames);
                byte[] bs = new byte[] { (byte)DragDropEffects.Move, 0, 0, 0 };
                using (MemoryStream ms = new MemoryStream(bs))
                {
                    data.SetData("Preferred DropEffect", ms);
                    Clipboard.Clear();
                    Clipboard.SetDataObject(data, true);
                }

                lblMgssage.Text = "クリップボードにファイルを切り取りました：'" + fullName + "'";
            }
        }

        public void CopyFile()
        {
            string fullName = (string)lstSearchResult.SelectedItem;
            if (fullName != null)
            {
                System.Collections.Specialized.StringCollection files = new System.Collections.Specialized.StringCollection();
                files.Add(fullName);
                Clipboard.SetFileDropList(files);

                lblMgssage.Text = "クリップボードにファイルをコピーしました：'" + fullName + "'";
            }
        }

        public void SelectExplorer()
        {
            string fullName = (string)lstSearchResult.SelectedItem;
            if (fullName != null)
            {
                if (Directory.Exists(fullName))
                {
                    Dictionary<string, string> conf = resultConfDictionary[fullName];
                    if (conf.ContainsKey(CONFKEY_BACLUP_DIR))
                    {
                        string shareDir = conf[CONFKEY_SHARE_DIR];
                        string backupShareDir = conf[CONFKEY_BACLUP_SHARE_DIR];

                        string normalizationFullName = Path.GetFullPath(fullName);
                        string partName = null;
                        string backupFullName = null;
                        if (String.Compare(conf[CONFKEY_COMPUTERNAME], Environment.MachineName, true) == 0 || shareDir == null || backupShareDir == null)
                        {
                            string dir = conf[CONFKEY_DIR];
                            string backupDir = conf[CONFKEY_BACLUP_DIR];

                            partName = normalizationFullName.Substring(dir.Length + 1);
                            backupFullName = Path.Combine(backupDir, partName);
                        }
                        else
                        {
                            partName = normalizationFullName.Substring(shareDir.Length + 1);
                            backupFullName = Path.Combine(backupShareDir, partName);
                        }

                        if (Directory.Exists(backupFullName))
                        {
                            Process.Start("explorer.exe", @"/select,""" + backupFullName + @"""");
                        }
                    }

                    Process.Start("explorer.exe", @"/select,""" + fullName + @"""");
                }
                else if (File.Exists(fullName))
                {
                    Process.Start("explorer.exe", @"/select,""" + fullName + @"""");
                }
            }
        }

        public void Rebuild()
        {
            ExecCommandSync(Config.Rebuild);
            LoadMyIndexes();
            SearchIndex(txtSearchText.Text);
            lblMgssage.Text = "インデックス再構築を完了しました。";
        }

        public void RebuildAll()
        {
            ExecCommandSync(Config.RebuildAll);
            LoadMyIndexes();
            SearchIndex(txtSearchText.Text);
            lblMgssage.Text = "全インデックス再構築を完了しました。";
        }

        /// <summary>
        /// コマンド起動
        /// </summary>
        /// <param name="command"></param>
        private void ExecCommandSync(string command)
        {
            string commandName = null;
            string commandArgs = null;
            Process process = null;

            if (command != "")
            {
                GetCommandNameAndArgs(command, ref commandName, ref commandArgs);

                if (commandArgs == null)
                {
                    try
                    {
                        process = Process.Start(commandName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("コマンドの実行に失敗しました。\nコマンド：" + commandName + "\n" + ex.Message, "コマンド実行エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        process = Process.Start(commandName, commandArgs);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("コマンドの実行に失敗しました。\nコマンド：" + commandName + "\n引数：" + commandArgs + "\n" + ex.Message, "コマンド実行エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (process != null)
                {
                    process.WaitForExit();
                }
            }
        }

        /// <summary>
        /// コマンド名と引数の取得
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandName"></param>
        /// <param name="commandArgs"></param>
        private void GetCommandNameAndArgs(string command, ref string commandName, ref string commandArgs)
        {
            // {filename}を含む場合は、"＜フルパスファイル名＞"(ダブルクォーテーション付き)と置換する
            string replaceCommand = ReplaceCommandValue(command);

            if (replaceCommand[0] == '"')
            {
                // コマンド名にダブルクォーテーションあり
                int secondQuotPos = replaceCommand.IndexOf('"', 1);
                if (secondQuotPos < 0)
                {
                    // 2番目のダブルクォーテーションなし(先頭のダブルクォーテーションを除いて全てコマンド名とする)
                    commandName = replaceCommand.Substring(1);
                    commandArgs = null;
                }
                else
                {
                    // 2番目のダブルクォーテーションあり
                    // 両側のダブルクォーテーションを省いてコマンド名とする
                    commandName = replaceCommand.Substring(1, secondQuotPos - 1);

                    int firstSpacePos = replaceCommand.IndexOf(' ', secondQuotPos + 1);
                    if (firstSpacePos < 0)
                    {
                        // 2番目のダブルクォーテーションの後に半角スペースなし(全てコマンド名)
                        commandName = replaceCommand;
                        commandArgs = null;
                    }
                    else
                    {
                        // 2番目のダブルクォーテーションの後に半角スペースあり(コマンド引数あり)
                        commandName = replaceCommand.Substring(0, firstSpacePos);
                        commandArgs = replaceCommand.Substring(firstSpacePos + 1);
                    }
                }
            }
            else
            {
                // コマンド名にダブルクォーテーションなし
                int firstSpacePos = replaceCommand.IndexOf(' ');
                if (firstSpacePos < 0)
                {
                    // 半角スペースなし(全てコマンド名)
                    commandName = replaceCommand;
                    commandArgs = null;
                }
                else
                {
                    // 半角スペースあり(コマンド引数あり)
                    commandName = replaceCommand.Substring(0, firstSpacePos);
                    commandArgs = replaceCommand.Substring(firstSpacePos + 1);
                }
            }
        }

        private string ReplaceCommandValue(string command)
        {
            string retCommand = command;

            retCommand = retCommand.Replace("{SearchMyIndex}", "\"" + Assembly.GetExecutingAssembly().Location + "\"");
            retCommand = retCommand.Replace("{SearchMyIndex-Dir}", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            return retCommand;
        }

        public void MakeMenu()
        {
            contextMenuStrip1.Items.Clear();

            ToolStripMenuItem tsmi1 = new ToolStripMenuItem();
            tsmi1.Text = Constant.MSG_DO_COPY_FULL_NAME;
            tsmi1.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoCopyFullName);
            tsmi1.Click += Operation.DoCopyFullName;
            contextMenuStrip1.Items.Add(tsmi1);

            ToolStripMenuItem tsmi2 = new ToolStripMenuItem();
            tsmi2.Text = Constant.MSG_DO_CUT_FILE;
            tsmi2.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoCutFile);
            tsmi2.Click += Operation.DoCutFile;
            contextMenuStrip1.Items.Add(tsmi2);

            ToolStripMenuItem tsmi3 = new ToolStripMenuItem();
            tsmi3.Text = Constant.MSG_DO_COPY_FILE;
            tsmi3.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoCopyFile);
            tsmi3.Click += Operation.DoCopyFile;
            contextMenuStrip1.Items.Add(tsmi3);

        }

        private string ReplaceShowMenuString(string str)
        {
            string retStr = str;
            retStr = retStr.Replace("Up", "↑");
            retStr = retStr.Replace("Down", "↓");
            retStr = retStr.Replace("Left", "←");
            retStr = retStr.Replace("Right", "→");
            retStr = retStr.Replace("NumPad", "テンキー");
            retStr = retStr.Replace("Decimal", "テンキー.");
            return retStr;
        }
    }
}
