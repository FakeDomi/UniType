using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace domi1819.UniType
{
    public partial class InputForm : Form
    {
        private readonly KeyboardHook hook = new KeyboardHook();

        private string inputText = string.Empty;
        private InputMode inputMode;

        protected override bool ShowWithoutActivation => true;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x0008; // WS_EX_TOPMOST

                return createParams;
            }
        }

        public InputForm()
        {
            this.InitializeComponent();

            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - 8 - this.Width, Screen.PrimaryScreen.WorkingArea.Bottom - 8 - this.Height);
            
            this.hook.OnKeyPress = this.HookKeyPress;
            this.hook.Install();

            RegisterHotKey(this.Handle, 0, 0x01, Keys.Add);
        }

        private bool HookKeyPress(Keys key)
        {
            switch (key)
            {
                case Keys.Add:
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

                    break;
                }
                case Keys.Escape:
                {
                    this.Hide();
                    this.hook.Active = false;

                    this.inputText = "";

                    this.RefreshPreview();

                    break;
                }
                case Keys.Back:
                {
                    if (this.inputText.Length > 0)
                    {
                        this.inputText = this.inputText.Substring(0, this.inputText.Length - 1);
                    }

                    this.RefreshPreview();

                    break;
                }
                case Keys.Enter:
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

                    break;
                }
                default:
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

                    break;
                }
            }

            return true;
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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            this.hook.Uninstall();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312) // WM_HOTKEY
            {
                if (!this.Visible)
                {
                    this.Show();
                    this.hook.Active = true;
                }
            }

            if (m.Msg == 0x0086) // WM_NCACTIVATE
            {
                m.WParam = new IntPtr(0x00); // FALSE
            }

            if (m.Msg != 0x0084) // WM_NCHITTEST
            {
                base.WndProc(ref m);
            }
        }

        private static void SendUnicodeChar(char inputChar, char? surrogateChar = null)
        {
            KeyboardInput input = new KeyboardInput { Type = 1, Vk = 0, Scan = inputChar, Time = 0, Flags = 0x0004, ExtraInfo = 0 };
            KeyboardInput[] inputs = surrogateChar == null ? new[] { input } : new[] { input, new KeyboardInput { Type = 1, Vk = 0, Scan = surrogateChar.Value, Time = 0, Flags = 0x0004, ExtraInfo = 0 } };

            SendInput(inputs.Length, inputs, Marshal.SizeOf(typeof(KeyboardInput)));
        }

        [DllImport("user32.dll")]
        private static extern uint SendInput(int numberOfInputs, KeyboardInput[] input, int sizeOfInputStructure);

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);

        [StructLayout(LayoutKind.Sequential, Size = 28)] // 8 unused bytes
        private struct KeyboardInput
        {
            internal uint Type;
            internal ushort Vk;
            internal ushort Scan;
            internal uint Flags;
            internal uint Time;
            internal uint ExtraInfo;
        }

        private enum InputMode
        {
            Unicode,
            Katakana
        }
    }
}
