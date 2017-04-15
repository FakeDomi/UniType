using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace domi1819.UniType
{
    internal class KeyboardHook
    {
        internal bool Active { get; set; }

        internal KeyPressCallback OnKeyPress { private get; set; }

        internal delegate bool KeyPressCallback(Keys key);

        private readonly KeyboardHookProc proc;
        private IntPtr hookId;

        private delegate IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam);

        internal KeyboardHook()
        {
            this.proc = this.KeyboardCallback;
        }

        internal void Install()
        {
            using (Process process = Process.GetCurrentProcess())
            using (ProcessModule module = process.MainModule)
            {
                this.hookId = SetWindowsHookEx(0x0D, this.proc, GetModuleHandle(module.ModuleName), 0);
            }
        }

        internal void Uninstall()
        {
            UnhookWindowsHookEx(this.hookId);
        }

        private IntPtr KeyboardCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            Keys key = (Keys)Marshal.ReadInt32(lParam);

            if (!this.Active || wParam.ToInt32() != 0x0100 || this.OnKeyPress == null)
            {
                return CallNextHookEx(this.hookId, nCode, wParam, lParam);
            }

            return this.OnKeyPress.Invoke(key) ? new IntPtr(-1) : CallNextHookEx(this.hookId, nCode, wParam, lParam);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern IntPtr UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
