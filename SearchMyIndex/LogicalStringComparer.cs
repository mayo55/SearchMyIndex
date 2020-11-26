using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SearchMyIndex
{
    /// <summary>
    /// 大文字小文字を区別せずに、
    /// 文字列内に含まれている数字を考慮して文字列を比較します。
    /// </summary>
    public class LogicalStringComparer :
        IComparer,
        IComparer<string>
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int StrCmpLogicalW(string x, string y);

        public int Compare(string x, string y)
        {
            return StrCmpLogicalW(x, y);
        }

        public int Compare(object x, object y)
        {
            return this.Compare(x.ToString(), y.ToString());
        }
    }
}
