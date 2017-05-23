using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static domi1819.UniType.WinConsts;

namespace domi1819.UniType
{
    public partial class InputForm : Form
    {
        private static readonly Color BorderColor = Color.FromArgb(67, 67, 70);

        private readonly KeyboardHook hook = new KeyboardHook();

        private string inputText = string.Empty;
        private InputMode inputMode;

        protected override bool ShowWithoutActivation => true;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= WS_EX_TOPMOST;

                return createParams;
            }
        }

        public InputForm()
        {
            this.InitializeComponent();

            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - 8 - this.Width, Screen.PrimaryScreen.WorkingArea.Bottom - 8 - this.Height);

            this.hook.OnKeyPress = this.HookKeyPress;
            this.hook.Install();

            User32.RegisterHotKey(this.Handle, 0, 0x01, Keys.Add);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                if (!this.Visible)
                {
                    this.Show();
                    this.hook.Active = true;
                }
            }

            if (m.Msg == WM_NCACTIVATE)
            {
                m.WParam = new IntPtr(FALSE);
            }

            if (m.Msg != WM_NCHITTEST)
            {
                base.WndProc(ref m);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            Message m = new Message { HWnd = this.Handle, Msg = WM_NCACTIVATE };

            this.WndProc(ref m);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            this.hook.Uninstall();
        }

        private bool HookKeyPress(Keys key)
        {
            switch (key)
            {
                case Keys.Add:
                    this.ProcessAddKey();
                    break;

                case Keys.Escape:
                    this.ProcessEscapeKey();
                    break;

                case Keys.Back:
                    this.ProcessBackKey();
                    break;

                case Keys.Enter:
                    this.ProcessEnterKey();
                    break;

                default:
                    this.ProcessDefaultKey(key);
                    break;
            }

            return true;
        }

        private void ProcessAddKey()
        {
            if (this.inputMode == InputMode.Unicode)
            {
                this.inputMode = InputMode.Katakana;
                this.modeLabel.Text = @"Ka";
            }
            else if (this.inputMode == InputMode.Katakana)
            {
                this.inputMode = InputMode.Unicode;
                this.modeLabel.Text = @"U+";
            }

            this.inputLabel.Text = "";
            this.RefreshPreview();
        }

        private void ProcessEscapeKey()
        {
            this.Hide();
            this.hook.Active = false;

            this.inputText = "";
            this.RefreshPreview();
        }

        private void ProcessBackKey()
        {
            if (this.inputText.Length > 0)
            {
                this.inputText = this.inputText.Substring(0, this.inputText.Length - 1);
            }

            this.RefreshPreview();
        }

        private void ProcessEnterKey()
        {
            try
            {
                this.hook.Active = false;

                if (this.inputMode == InputMode.Unicode)
                {
                    string unicodeText = char.ConvertFromUtf32(int.Parse(this.inputText, NumberStyles.HexNumber));

                    SendUnicodeChar(unicodeText[0], unicodeText.Length > 1 ? (char?)unicodeText[1] : null);
                }
                else if (this.inputMode == InputMode.Katakana)
                {
                    if (KatakanaMapping.Mapping.TryGetValue(this.inputText, out char character))
                    {
                        SendUnicodeChar(character);
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // Whatever
            }
            catch (FormatException)
            {
                // Whatever
            }

            this.hook.Active = true;
            this.inputText = "";
            this.RefreshPreview();
        }

        private void ProcessDefaultKey(Keys key)
        {
            int input = (int)key;

            if (input >= 96 && input <= 105)
            {
                input -= 48;
            }

            if (input >= 65 && input <= 90)
            {
                input += 32;
            }

            if (this.inputMode == InputMode.Unicode)
            {
                if (input >= 48 && input <= 57 || input >= 97 && input <= 102)
                {
                    this.AppendInput((char)input);
                }
            }
            else if (this.inputMode == InputMode.Katakana)
            {
                if (input >= 48 && input <= 57 || input >= 97 && input <= 122)
                {
                    this.AppendInput((char)input);
                }
            }
        }

        private void RefreshPreview()
        {
            this.previewLabel.Text = "";
            this.inputLabel.Text = this.inputText;

            try
            {
                if (this.inputMode == InputMode.Unicode)
                {
                    this.previewLabel.Text = char.ConvertFromUtf32(int.Parse(this.inputText, NumberStyles.HexNumber));
                }
                else if (this.inputMode == InputMode.Katakana)
                {
                    if (KatakanaMapping.Mapping.TryGetValue(this.inputText, out char character))
                    {
                        this.previewLabel.Text = character.ToString();
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // Whatever
            }
            catch (FormatException)
            {
                // Whatever
            }
        }

        private void AppendInput(char c)
        {
            if (this.inputText.Length < 6)
            {
                this.inputText += c;
            }

            this.RefreshPreview();
        }

        private void LabelsPaint(object sender, PaintEventArgs e)
        {
            if (sender is Control control)
            {
                ControlPaint.DrawBorder(e.Graphics, control.ClientRectangle, BorderColor, ButtonBorderStyle.Solid);
            }
        }

        private static void SendUnicodeChar(char inputChar, char? surrogateChar = null)
        {
            User32.KeyboardInput input = new User32.KeyboardInput { Type = 1, Vk = 0, Scan = inputChar, Time = 0, Flags = 0x0004, ExtraInfo = 0 };
            User32.KeyboardInput[] inputs = surrogateChar == null ? new[] { input } : new[] { input, new User32.KeyboardInput { Type = 1, Vk = 0, Scan = surrogateChar.Value, Time = 0, Flags = 0x0004, ExtraInfo = 0 } };

            User32.SendInput(inputs.Length, inputs, Marshal.SizeOf(typeof(User32.KeyboardInput)));
        }

        private enum InputMode
        {
            Unicode,
            Katakana
        }
    }
}
