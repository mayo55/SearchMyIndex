using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace SearchMyIndex
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class Config
    {
        public static string Rebuild { get; set;  }
        public static string RebuildAll { get; set;  }

        public static void LoadConfig()
        {
            KeyManager.Initalize();

            Rebuild = Properties.Settings.Default.ExecCommand1;
            RebuildAll = Properties.Settings.Default.ExecCommand2;

            KeyManager.AddKeyEvents(Properties.Settings.Default.DoCopyFullName, Operation.DoCopyFullName);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoCutFile, Operation.DoCutFile);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoCopyFile, Operation.DoCopyFile);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoSelectExplorer, Operation.DoSelectExplorer);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoRebuild, Operation.DoRebuld);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoRebuildAll, Operation.DoRebuildAll);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoConfig, Operation.DoConfig);
        }
    }
}
