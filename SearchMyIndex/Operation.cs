using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace SearchMyIndex
{
    public class Operation
    {

        public static void DoCopyFullName(object sender, EventArgs e)
        {
            Form1 form1 = GetForm(sender);
            form1.CopyFullName();
        }

        public static void DoCutFile(object sender, EventArgs e)
        {
            Form1 form1 = GetForm(sender);
            form1.CutFile();
        }

        public static void DoCopyFile(object sender, EventArgs e)
        {
            Form1 form1 = GetForm(sender);
            form1.CopyFile();
        }

        public static void DoSelectExplorer(object sender, EventArgs e)
        {
            Form1 form1 = GetForm(sender);
            form1.SelectExplorer();
        }

        public static void DoRebuld(object sender, EventArgs e)
        {
            Form1 form1 = GetForm(sender);
            form1.Rebuild();
        }

        public static void DoRebuildAll(object sender, EventArgs e)
        {
            Form1 form1 = GetForm(sender);
            form1.RebuildAll();
        }

        public static void DoConfig(object sender, EventArgs e)
        {
            ConfigForm configForm = new ConfigForm();
            configForm.ShowDialog();
        }

        private static Form1 GetForm(Object sender)
        {
            Form1 form = null;
            if (sender is ToolStripMenuItem tsmi)
            {
                ContextMenuStrip cms = null;
                // サブメニュー一段だけ対応
                if (tsmi.Owner is ToolStripDropDownMenu tsddm)
                {
                    if (tsddm.OwnerItem == null)
                    {
                        cms = (ContextMenuStrip)tsmi.Owner;
                    }
                    else
                    {
                        cms = (ContextMenuStrip)tsddm.OwnerItem.Owner;
                    }
                }
                Control c = cms.SourceControl;
                form = (Form1)c.FindForm();
            }
            else
            {
                form = (Form1)sender;
            }

            return form;
        }
    }
}
