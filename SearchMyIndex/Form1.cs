using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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

        private List<Dictionary<string, string>> confList = new List<Dictionary<string, string>>();
        private List<string> fullNameList = new List<string>();

        private Dictionary<string, Dictionary<string, string>> resultConfDictionary = new Dictionary<string, Dictionary<string, string>>();

        public Form1()
        {
            InitializeComponent();

            LoadMyIndexes();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Location = Properties.Settings.Default.Form1Location;
            Size = Properties.Settings.Default.Form1Size;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Form1Location = Location;
            Properties.Settings.Default.Form1Size = Size;
            Properties.Settings.Default.Save();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (ActiveControl == lstSearchResult && lstSearchResult.SelectedIndex <= 0)
                {
                    txtSearchText.Focus();
                }
            }

            if (e.KeyCode == Keys.Down)
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
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void LoadMyIndexes()
        {
            string[] indexes = Directory.GetFiles(Properties.Settings.Default.MyIndexDir);

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

            for (int i = 0; i < fullNameList.Count; i++)
            {
                string fullName = fullNameList[i];

                if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(fullName, searchStr, CompareOptions.IgnoreCase | CompareOptions.IgnoreWidth | CompareOptions.IgnoreKanaType) >= 0)
                {
                    searchResultList.Add(fullName);

                    Dictionary<string, string> conf = confList[i];
                    resultConfDictionary.Add(fullName, conf);
                }
            }

            searchResultList.Sort();

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
                        //Process.Start("explorer.exe", @"/select,""" + backupFullName + @"""");
                        Process.Start(backupFullName);
                    }
                }

                //Process.Start("explorer.exe", @"/select,""" + fullName + @"""");
                Process.Start(fullName);
            }
            else if (File.Exists(fullName))
            {
                Process.Start(fullName);
            }
        }
    }
}
