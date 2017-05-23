using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace domi1819.UniType
{
    internal static class User32
    {
        internal delegate IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        internal static extern IntPtr UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        internal static extern uint SendInput(int numberOfInputs, KeyboardInput[] input, int sizeOfInputStructure);

        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);

        [StructLayout(LayoutKind.Sequential, Size = 28)] // 8 unused bytes
        internal struct KeyboardInput
        {
            internal uint Type;
            internal ushort Vk;
            internal ushort Scan;
            internal uint Flags;
            internal uint Time;
            internal uint ExtraInfo;
        }

    }
}
