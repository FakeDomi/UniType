using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace domi1819.UniType
{
    public partial class InputForm : Form
    {
        private bool focusHack;
        private InputMode inputMode;

        private readonly Dictionary<string, string> katakanaMapping = new Dictionary<string, string>();

        private static string katakana = @"a=ア
i=イ
u=ウ
e=エ
o=オ
ka=カ
ki=キ
ku=ク
ke=ケ
ko=コ
sa=サ
shi=シ
su=ス
se=セ
so=ソ
ta=タ
chiチ
tsu=ツ
te=テ
to=ト
na=ナ
ni=ニ
nu=ヌ
ne=ネ
no=ノ
ha=ハ
hi=ヒ
fu=フ
he=ヘ
ho=ホ
ma=マ
mi=ミ
mu=ム
me=メ
mo=モ
ya=ヤ
yu=ユ
yo=ヨ
ra=ラ
ri=リ
ru=ル
re=レ
ro=ロ
wa=ワ
wi=ヰ
we=ヱ
wo=ヲ
ga=ガ
gi=ギ
gu=グ
ge=ゲ
go=ゴ
za=ザ
ji=ジ
zu=ズ
ze=ゼ
zo=ゾ
da=ダ
di=ヂ
du=ヅ
de=デ
do=ド
ba=バ
bi=ビ
bu=ブ
be=ベ
bo=ボ
pa=パ
pi=ピ
pu=プ
pe=ペ
po=ポ
n=ン
-=ー
.=・
 =・";

        public InputForm()
        {
            this.InitializeComponent();
            
            this.Location = new Point(
            Screen.PrimaryScreen.WorkingArea.Right - 8 - this.Width, Screen.PrimaryScreen.WorkingArea.Bottom - 8 - this.Height);

            this.uiInputTextBox.TextChanged += this.HandleTextBoxTextChanged;
            this.uiInputTextBox.LostFocus += this.HandleTextBoxLostFocus;
            this.uiInputTextBox.KeyPress += this.HandleTextBoxKeyPress;

            RegisterHotKey(this.Handle, 0, 0x0001, (uint) Keys.Add);

            //using (StreamReader reader = new StreamReader("katakana.txt"))
            //{
            //    while (!reader.EndOfStream)
            //    {
            //        string[] line = (reader.ReadLine() ?? "").Split('=');

            //        if (line.Length == 2)
            //        {
            //            this.katakanaMapping.Add(line[0], line[1]);
            //        }
            //    }
            //}

            string[] split = katakana.Split('\n');

            foreach (var element in split)
            {
                string[] line = element.Split('=');

                if (line.Length == 2)
                {
                    this.katakanaMapping.Add(line[0], line[1]);
                }
            }
        }

        protected override void OnShown(EventArgs e)
        {
            this.Hide();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312) // WM_HOTKEY
            {
                if (!this.Visible)
                {
                    this.focusHack = true;
                    this.Show();
                    this.Activate();
                    this.uiInputTextBox.Text = "";
                    this.focusHack = false;
                }
            }

            if (m.Msg != 0x0084) // WM_NCHITTEST
            {
                base.WndProc(ref m);
            }
        }

        private void HandleTextBoxTextChanged(object sender, EventArgs e)
        {
            this.uiPreviewLabel.Text = "";

            try
            {
                if (this.inputMode == InputMode.Unicode)
                {
                    this.uiPreviewLabel.Text = char.ConvertFromUtf32(int.Parse(this.uiInputTextBox.Text, NumberStyles.HexNumber));
                }
                else if (this.inputMode == InputMode.Katakana)
                {
                    if (this.katakanaMapping.ContainsKey(this.uiInputTextBox.Text))
                    {
                        this.uiPreviewLabel.Text = this.katakanaMapping[this.uiInputTextBox.Text];
                    }
                }
            }
            catch (Exception)
            {
                // Ignored
            }
        }

        private void HandleTextBoxLostFocus(object sender, EventArgs e)
        {
            if (!this.focusHack)
            {
                this.Hide();
            }
        }

        private void HandleTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '+')
            {
                if (this.inputMode == InputMode.Unicode)
                {
                    this.inputMode = InputMode.Katakana;
                    this.uiUnicodePlusLabel.Text = @"Ka";
                }
                else if (this.inputMode == InputMode.Katakana)
                {
                    this.inputMode = InputMode.Unicode;
                    this.uiUnicodePlusLabel.Text = @"U+";
                }

                this.uiInputTextBox.Text = "";

                e.Handled = true;
                return;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                this.Hide();

                try
                {
                    if (this.inputMode == InputMode.Unicode)
                    {
                        SendUnicodeText(char.ConvertFromUtf32(int.Parse(this.uiInputTextBox.Text, NumberStyles.HexNumber)));
                    }
                    else if (this.inputMode == InputMode.Katakana)
                    {
                        if (this.katakanaMapping.ContainsKey(this.uiInputTextBox.Text))
                        {
                            SendUnicodeText(this.katakanaMapping[this.uiInputTextBox.Text]);
                        }

                        Thread.Sleep(150);

                        this.focusHack = true;
                        this.Show();
                        this.Activate();
                        this.uiInputTextBox.Text = "";
                        this.focusHack = false;
                    }
                }
                catch (Exception)
                {
                    // Ignored
                }
            }
            else if (e.KeyChar == (char) Keys.Escape)
            {
                this.Hide();
            }
           
            if (e.KeyChar != '\b' && this.inputMode == InputMode.Unicode)
            {
                e.Handled = this.uiInputTextBox.Text.Length >= 6 || !(e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar >= 'a' && e.KeyChar <= 'f' || e.KeyChar >= 'A' && e.KeyChar <= 'F');
            }
        }

        private static void SendUnicodeText(string text)
        {
            foreach (char utf16Char in text)
            {
                INPUT keyInput = new INPUT { Type = 1, ki = { Vk = 0, Scan = utf16Char, Time = 0, Flags = 0x0004, ExtraInfo = 0 } };
                INPUT[] input = { keyInput, keyInput };

                SendInput(1, input, Marshal.SizeOf(typeof(INPUT)));
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 24)]
        // ReSharper disable once InconsistentNaming
        private struct KEYBDINPUT
        {
            internal ushort Vk;
            internal ushort Scan;
            internal uint Flags;
            internal uint Time;
            internal uint ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        // ReSharper disable once InconsistentNaming
        private struct INPUT
        {
            internal int Type;
            internal KEYBDINPUT ki;
        }

        [DllImport("user32", CharSet = CharSet.Unicode)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] input, int sizeOfInputStructure);

        [DllImport("user32")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        private enum InputMode
        {
            Unicode, Katakana
        }
    }
}
