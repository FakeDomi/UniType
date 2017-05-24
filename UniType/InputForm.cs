using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static domi1819.UniType.WinConsts;

namespace domi1819.UniType
{
    public partial class InputForm : Form
    {
        private static readonly Color BorderColor = Color.FromArgb(67, 67, 70);

        private readonly KeyboardHook hook = new KeyboardHook();
        private readonly InputModes modes = new InputModes();

        private string inputText = string.Empty;

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
            this.modes.Next();
            this.modeLabel.Text = this.modes.Current.Text;

            this.inputText = "";
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
            this.hook.Active = false;

            try
            {
                Tuple<char, char?> chars = this.modes.Current.GetResult(this.inputText);

                if (chars != null)
                {
                    SendUnicodeChar(chars.Item1, chars.Item2);
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
            int scanCode = (int)key;

            if (scanCode >= 96 && scanCode <= 105)
            {
                scanCode -= 48;
            }

            if (scanCode >= 65 && scanCode <= 90)
            {
                scanCode += 32;
            }

            if (this.modes.Current.AcceptsKey(scanCode))
            {
                this.AppendInput((char)scanCode);
            }
        }

        private void RefreshPreview()
        {
            this.previewLabel.Text = "";
            this.inputLabel.Text = this.inputText;

            try
            {
                this.previewLabel.Text = this.modes.Current.GetPreview(this.inputText);
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

        private static void SendUnicodeChar(char inputChar, char? surrogateChar)
        {
            User32.KeyboardInput input = new User32.KeyboardInput { Type = INPUT_KEYBOARD, Scan = inputChar, Flags = KEYEVENTF_UNICODE };
            User32.KeyboardInput[] inputs = surrogateChar == null ? new[] { input } : new[] { input, new User32.KeyboardInput { Type = INPUT_KEYBOARD, Scan = surrogateChar.Value, Flags = KEYEVENTF_UNICODE } };

            User32.SendInput(inputs.Length, inputs, Marshal.SizeOf(typeof(User32.KeyboardInput)));
        }
    }
}
