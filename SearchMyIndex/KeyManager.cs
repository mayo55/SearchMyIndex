using System;
using System.Collections.Generic;
using System.Windows.Forms;

public delegate void KeyManagerDelegate(object sender, EventArgs e);

namespace SearchMyIndex
{
    public class KeyManager
    {
        private static Dictionary<Keys, List<KeyManagerDelegate>> keyDownMap = new Dictionary<Keys, List<KeyManagerDelegate>>();

        private static Dictionary<char, List<KeyManagerDelegate>> keyPressMap = new Dictionary<char, List<KeyManagerDelegate>>();
        public static void AddKeyEvent(string keyStr, KeyManagerDelegate addr)
        {
            dynamic keyStrArray = keyStr.Split(',');
            foreach (string keyStr2 in keyStrArray)
            {
                if (keyStr2.Length == 1)
                {
                    char keyStrChar = keyStr2.ToCharArray(0, 1)[0];
                    char mapKey = char.ToLower(keyStrChar);
                    if (!keyPressMap.ContainsKey(mapKey))
                    {
                        keyPressMap[mapKey] = new List<KeyManagerDelegate>();
                    }
                    keyPressMap[mapKey].Add(addr);
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
                                MessageBox.Show("キー名称の指定が正しくありません。[" + keyStr2 + "]", "コンフィグファイルエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                throw new Exception("キー名称の指定が正しくありません。");
                        }
                    }

                    if (!keyDownMap.ContainsKey(keyData))
                    {
                        keyDownMap[keyData] = new List<KeyManagerDelegate>();
                    }
                    keyDownMap[keyData].Add(addr);
                }
            }
        }

        public static void AddKeyEvents(string keyStrs, KeyManagerDelegate addr)
        {
            string[] keyStrArray = keyStrs.Split(',');
            foreach (string keyStr in keyStrArray)
            {
                AddKeyEvent(keyStr, addr);
            }
        }

        public static List<KeyManagerDelegate> GetKeyDown(Keys keys)
        {
            try
            {
                return keyDownMap[keys];
            }
            catch (KeyNotFoundException e)
            {
                return null;
            }
        }

        public static List<KeyManagerDelegate> GetKeyPress(char keyStr)
        {
            try
            {
                return keyPressMap[char.ToLower(keyStr)];
            }
            catch (KeyNotFoundException e)
            {
                return null;
            }
        }

        public static void Initalize()
        {
            keyDownMap = new Dictionary<Keys, List<KeyManagerDelegate>>();
            keyPressMap = new Dictionary<char, List<KeyManagerDelegate>>();
        }
    }
}
