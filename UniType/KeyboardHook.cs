using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static domi1819.UniType.WinConsts;

namespace domi1819.UniType
{
    internal class KeyboardHook
    {
        internal bool Active { private get; set; }

        internal KeyPressCallback OnKeyPress { private get; set; }

        internal delegate bool KeyPressCallback(Keys key);

        private readonly User32.KeyboardHookProc proc;
        private IntPtr hookId;

        internal KeyboardHook()
        {
            this.proc = this.KeyboardCallback;
        }

        internal void Install()
        {
            using (Process process = Process.GetCurrentProcess())
            using (ProcessModule module = process.MainModule)
            {
                this.hookId = User32.SetWindowsHookEx(WH_KEYBOARD_LL, this.proc, User32.GetModuleHandle(module.ModuleName), 0);
            }
        }

        internal void Uninstall()
        {
            User32.UnhookWindowsHookEx(this.hookId);
        }

        private IntPtr KeyboardCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            Keys key = (Keys)Marshal.ReadInt32(lParam);

            if (!this.Active || wParam.ToInt32() != 0x0100 || this.OnKeyPress == null)
            {
                return User32.CallNextHookEx(this.hookId, nCode, wParam, lParam);
            }

            return this.OnKeyPress.Invoke(key) ? new IntPtr(-1) : User32.CallNextHookEx(this.hookId, nCode, wParam, lParam);
        }
    }
}
