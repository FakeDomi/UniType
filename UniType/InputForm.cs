using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace domi1819.UniType
{
    public partial class InputForm : Form
    {
        private bool focusHack;

        public InputForm()
        {
            this.InitializeComponent();
            
            this.Location = new Point(
            Screen.PrimaryScreen.WorkingArea.Right - 8 - this.Width, Screen.PrimaryScreen.WorkingArea.Bottom - 8 - this.Height);

            this.uiInputTextBox.TextChanged += this.HandleTextBoxTextChanged;
            this.uiInputTextBox.LostFocus += this.HandleTextBoxLostFocus;
            this.uiInputTextBox.KeyPress += this.HandleTextBoxKeyPress;

            RegisterHotKey(this.Handle, 0, 0x0001, (uint) Keys.Add);
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
                this.uiPreviewLabel.Text = char.ConvertFromUtf32(int.Parse(this.uiInputTextBox.Text, NumberStyles.HexNumber));
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
            if (e.KeyChar == (char) Keys.Enter)
            {
                this.Hide();

                try
                {
                    SendUnicodeText(char.ConvertFromUtf32(int.Parse(this.uiInputTextBox.Text, NumberStyles.HexNumber)));
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
           
            if (e.KeyChar != '\b')
            {
                if (this.uiInputTextBox.Text.Length >= 6)
                {
                    e.Handled = true;
                    return;
                }

                switch (e.KeyChar)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                        break;
                    default:
                        e.Handled = true;
                        break;
                }
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
    }
}
