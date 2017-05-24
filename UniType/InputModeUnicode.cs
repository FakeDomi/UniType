using System;
using System.Globalization;

namespace domi1819.UniType
{
    internal class InputModeUnicode : IInputMode
    {
        public string Text => "U+";

        public bool AcceptsKey(int scanCode)
        {
            return scanCode >= 48 && scanCode <= 57 || scanCode >= 97 && scanCode <= 102;
        }

        public string GetPreview(string data)
        {
            return char.ConvertFromUtf32(int.Parse(data, NumberStyles.HexNumber));
        }

        public Tuple<char, char?> GetResult(string data)
        {
            string unicodeText = char.ConvertFromUtf32(int.Parse(data, NumberStyles.HexNumber));

            return new Tuple<char, char?>(unicodeText[0], unicodeText.Length > 1 ? (char?)unicodeText[1] : null);
        }
    }
}
