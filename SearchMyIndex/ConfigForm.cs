using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchMyIndex
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            MinimizeBox = false;
            KeyPreview = true;

            dataGridView1.ColumnCount = 3;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.Columns[0].Name = "key";
            dataGridView1.Columns[0].HeaderText = "項目";
            dataGridView1.Columns[0].ReadOnly = true;

            dataGridView1.Columns[1].Name = "value";
            dataGridView1.Columns[1].HeaderText = "値";

            dataGridView1.Columns[2].Name = "id";
            dataGridView1.Columns[2].Visible = false;

            dataGridView1.Rows.Add(Constant.MSG_KEY_CONFIG, null, null);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGreen;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;

            dataGridView1.Rows.Add(Constant.MSG_DO_ALL, null, null);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightCyan;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;

            dataGridView1.Rows.Add(Constant.MSG_DO_COPY_FULL_NAME, Properties.Settings.Default.DoCopyFullName, Constant.ID_DO_COPY_FULL_NAME);
            dataGridView1.Rows.Add(Constant.MSG_DO_COPY_FILE, Properties.Settings.Default.DoCopyFile, Constant.ID_DO_COPY_FILE);
            dataGridView1.Rows.Add(Constant.MSG_DO_SELECT_EXPLORER, Properties.Settings.Default.DoSelectExplorer, Constant.ID_DO_SELECT_EXPLORER);
            dataGridView1.Rows.Add(Constant.MSG_DO_REBUILD, Properties.Settings.Default.DoRebuild, Constant.ID_DO_REBUILD);
            dataGridView1.Rows.Add(Constant.MSG_DO_REBUILD_ALL, Properties.Settings.Default.DoRebuildAll, Constant.ID_DO_REBUILD_ALL);


            dataGridView1.Rows.Add(Constant.MSG_CONFIG, null, null);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGreen;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;

            dataGridView1.Rows.Add(Constant.MSG_EXEC_COMMAND_1, Properties.Settings.Default.ExecCommand1, Constant.ID_REBUILD);
            dataGridView1.Rows.Add(Constant.MSG_EXEC_COMMAND_2, Properties.Settings.Default.ExecCommand2, Constant.ID_REBUILD_ALL);

            dataGridView1.CurrentCell = dataGridView1["value", 2];

            lblMessage.Text = null;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // 設定値を保存
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string id = (string)dataGridView1["id", i].Value;
                string value = (string)dataGridView1["value", i].Value;

                switch (id)
                {
                    case Constant.ID_DO_CONFIG:
                        Properties.Settings.Default.DoConfig = value;
                        break;

                    case Constant.ID_DO_COPY_FULL_NAME:
                        Properties.Settings.Default.DoCopyFullName = value;
                        break;

                    case Constant.ID_DO_COPY_FILE:
                        Properties.Settings.Default.DoCopyFile = value;
                        break;

                    case Constant.ID_DO_SELECT_EXPLORER:
                        Properties.Settings.Default.DoSelectExplorer = value;
                        break;

                    case Constant.ID_DO_REBUILD:
                        Properties.Settings.Default.DoRebuild = value;
                        break;

                    case Constant.ID_DO_REBUILD_ALL:
                        Properties.Settings.Default.DoRebuildAll = value;
                        break;

                    case Constant.ID_REBUILD:
                        Properties.Settings.Default.ExecCommand1 = value;
                        break;

                    case Constant.ID_REBUILD_ALL:
                        Properties.Settings.Default.ExecCommand2 = value;
                        break;

                }
            }

            Properties.Settings.Default.Save();
            Config.LoadConfig();

            DialogResult = DialogResult.OK;
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            if (e.ColumnIndex != 1)
            {
                return;
            }

            string id = (string)dataGridView["id", e.RowIndex].Value;
            string formattedValue = e.FormattedValue.ToString();
            bool error = false;
            int intValue;
            float floatValue;
            bool boolValue;

            switch (id)
            {
                case Constant.ID_DO_CONFIG:
                case Constant.ID_DO_COPY_FULL_NAME:
                case Constant.ID_DO_CUT_FILE:
                case Constant.ID_DO_COPY_FILE:
                case Constant.ID_DO_SELECT_EXPLORER:
                case Constant.ID_DO_REBUILD:
                case Constant.ID_DO_REBUILD_ALL:
                    if (!CheckKeyConfig(formattedValue))
                    {
                        lblMessage.Text = "キーの指定が正しくありません。";
                        e.Cancel = true;
                    }
                    break;
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            string id = (string)dataGridView["id", e.RowIndex].Value;

            switch (id)
            {
                case Constant.ID_REBUILD:
                case Constant.ID_REBUILD_ALL:
                    lblMessage.Text = "コマンド指定 空で指定なし";
                    break;

            }
        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            lblMessage.Text = null;
        }

        private bool CheckKeyConfig(string keyStr)
        {
            dynamic keyStrArray = keyStr.Split(',');
            foreach (string keyStr2 in keyStrArray)
            {
                if (keyStr2.Length == 1)
                {
                    continue;
                }
                else
                {
                    Keys keyData = 0;

                    string[] keyArray = keyStr2.Split('+');
                    foreach (string key in keyArray)
                    {
                        switch (key)
                        {
                            case "Control":
                            case "Ctrl":
                                keyData |= Keys.Control;
                                break;
                            case "Alt":
                                keyData |= Keys.Alt;
                                break;
                            case "Shift":
                                keyData |= Keys.Shift;
                                break;
                            case "Escape":
                            case "Esc":
                                keyData |= Keys.Escape;
                                break;
                            case "Left":
                                keyData |= Keys.Left;
                                break;
                            case "Right":
                                keyData |= Keys.Right;
                                break;
                            case "Up":
                                keyData |= Keys.Up;
                                break;
                            case "Down":
                                keyData |= Keys.Down;
                                break;
                            case "NumPad0":
                                keyData |= Keys.NumPad0;
                                break;
                            case "NumPad1":
                                keyData |= Keys.NumPad1;
                                break;
                            case "NumPad2":
                                keyData |= Keys.NumPad2;
                                break;
                            case "NumPad3":
                                keyData |= Keys.NumPad3;
                                break;
                            case "NumPad4":
                                keyData |= Keys.NumPad4;
                                break;
                            case "NumPad5":
                                keyData |= Keys.NumPad5;
                                break;
                            case "NumPad6":
                                keyData |= Keys.NumPad6;
                                break;
                            case "NumPad7":
                                keyData |= Keys.NumPad7;
                                break;
                            case "NumPad8":
                                keyData |= Keys.NumPad8;
                                break;
                            case "NumPad9":
                                keyData |= Keys.NumPad9;
                                break;
                            case "Back":
                                keyData |= Keys.Back;
                                break;
                            case "CapsLock":
                                keyData |= Keys.CapsLock;
                                break;
                            case "Decimal":
                                keyData |= Keys.Decimal;
                                break;
                            case "Divide":
                                keyData |= Keys.Divide;
                                break;
                            case "End":
                                keyData |= Keys.End;
                                break;
                            case "Enter":
                                keyData |= Keys.Enter;
                                break;
                            case "F1":
                                keyData |= Keys.F1;
                                break;
                            case "F2":
                                keyData |= Keys.F2;
                                break;
                            case "F3":
                                keyData |= Keys.F3;
                                break;
                            case "F4":
                                keyData |= Keys.F4;
                                break;
                            case "F5":
                                keyData |= Keys.F5;
                                break;
                            case "F6":
                                keyData |= Keys.F6;
                                break;
                            case "F7":
                                keyData |= Keys.F7;
                                break;
                            case "F8":
                                keyData |= Keys.F8;
                                break;
                            case "F9":
                                keyData |= Keys.F9;
                                break;
                            case "F10":
                                keyData |= Keys.F10;
                                break;
                            case "F11":
                                keyData |= Keys.F11;
                                break;
                            case "F12":
                                keyData |= Keys.F12;
                                break;
                            case "FinalMode":
                                keyData |= Keys.FinalMode;
                                break;
                            case "Home":
                                keyData |= Keys.Home;
                                break;
                            case "IMEConvert":
                                keyData |= Keys.IMEConvert;
                                break;
                            case "IMEModeChange":
                                keyData |= Keys.IMEModeChange;
                                break;
                            case "IMENonconvert":
                                keyData |= Keys.IMENonconvert;
                                break;
                            case "Insert":
                                keyData |= Keys.Insert;
                                break;
                            case "KanaMode":
                                keyData |= Keys.KanaMode;
                                break;
                            case "KanjiMode":
                                keyData |= Keys.KanjiMode;
                                break;
                            case "LControlKey":
                                keyData |= Keys.LControlKey;
                                break;
                            case "LMenu":
                                keyData |= Keys.LMenu;
                                break;
                            case "LShiftKey":
                                keyData |= Keys.LShiftKey;
                                break;
                            case "LWin":
                                keyData |= Keys.LWin;
                                break;
                            case "MButton":
                                keyData |= Keys.MButton;
                                break;
                            case "Menu":
                                keyData |= Keys.Menu;
                                break;
                            case "Multiply":
                                keyData |= Keys.Multiply;
                                break;
                            case "Next":
                                keyData |= Keys.Next;
                                break;
                            case "NumLock":
                                keyData |= Keys.NumLock;
                                break;
                            case "PageDown":
                                keyData |= Keys.PageDown;
                                break;
                            case "PageUp":
                                keyData |= Keys.PageUp;
                                break;
                            case "Pause":
                                keyData |= Keys.Pause;
                                break;
                            case "PrintScreen":
                                keyData |= Keys.PrintScreen;
                                break;
                            case "RButton":
                                keyData |= Keys.RButton;
                                break;
                            case "RMenu":
                                keyData |= Keys.RMenu;
                                break;
                            case "RShiftKey":
                                keyData |= Keys.RShiftKey;
                                break;
                            case "RWin":
                                keyData |= Keys.RWin;
                                break;
                            case "Scroll":
                                keyData |= Keys.Scroll;
                                break;
                            case "ShiftKey":
                                keyData |= Keys.ShiftKey;
                                break;
                            case "Space":
                                keyData |= Keys.Space;
                                break;
                            case "Subtract":
                                keyData |= Keys.Subtract;
                                break;
                            case "Tab":
                                keyData |= Keys.Tab;
                                break;
                            case "XButton1":
                                keyData |= Keys.XButton1;
                                break;
                            case "XButton2":
                                keyData |= Keys.XButton2;
                                break;
                            case "A":
                            case "a":
                                keyData |= Keys.A;
                                break;
                            case "B":
                            case "b":
                                keyData |= Keys.B;
                                break;
                            case "C":
                            case "c":
                                keyData |= Keys.C;
                                break;
                            case "D":
                            case "d":
                                keyData |= Keys.D;
                                break;
                            case "E":
                            case "e":
                                keyData |= Keys.E;
                                break;
                            case "F":
                            case "f":
                                keyData |= Keys.F;
                                break;
                            case "G":
                            case "g":
                                keyData |= Keys.G;
                                break;
                            case "H":
                            case "h":
                                keyData |= Keys.H;
                                break;
                            case "I":
                            case "i":
                                keyData |= Keys.I;
                                break;
                            case "J":
                            case "j":
                                keyData |= Keys.J;
                                break;
                            case "K":
                            case "k":
                                keyData |= Keys.K;
                                break;
                            case "L":
                            case "l":
                                keyData |= Keys.L;
                                break;
                            case "M":
                            case "m":
                                keyData |= Keys.M;
                                break;
                            case "N":
                            case "n":
                                keyData |= Keys.N;
                                break;
                            case "O":
                            case "o":
                                keyData |= Keys.O;
                                break;
                            case "P":
                            case "p":
                                keyData |= Keys.P;
                                break;
                            case "Q":
                            case "q":
                                keyData |= Keys.Q;
                                break;
                            case "R":
                            case "r":
                                keyData |= Keys.R;
                                break;
                            case "S":
                            case "s":
                                keyData |= Keys.S;
                                break;
                            case "T":
                            case "t":
                                keyData |= Keys.T;
                                break;
                            case "U":
                            case "u":
                                keyData |= Keys.U;
                                break;
                            case "V":
                            case "v":
                                keyData |= Keys.V;
                                break;
                            case "W":
                            case "w":
                                keyData |= Keys.W;
                                break;
                            case "X":
                            case "x":
                                keyData |= Keys.X;
                                break;
                            case "Y":
                            case "y":
                                keyData |= Keys.Y;
                                break;
                            case "Z":
                            case "z":
                                keyData |= Keys.Z;
                                break;
                            case "0":
                                keyData |= Keys.D0;
                                break;
                            case "1":
                                keyData |= Keys.D1;
                                break;
                            case "2":
                                keyData |= Keys.D2;
                                break;
                            case "3":
                                keyData |= Keys.D3;
                                break;
                            case "4":
                                keyData |= Keys.D4;
                                break;
                            case "5":
                                keyData |= Keys.D5;
                                break;
                            case "6":
                                keyData |= Keys.D6;
                                break;
                            case "7":
                                keyData |= Keys.D7;
                                break;
                            case "8":
                                keyData |= Keys.D8;
                                break;
                            case "9":
                                keyData |= Keys.D9;
                                break;
                            default:
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        private string GetGridValue(string id)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string gridId = (string)dataGridView1["id", i].Value;
                if (gridId == id)
                {
                    return (string)dataGridView1["value", i].Value;
                }
            }

            return null;
        }

        private void ConfigForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataGridView1.IsCurrentCellInEditMode)
            {
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
