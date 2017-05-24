using System;

namespace domi1819.UniType
{
    internal class InputModeKatakana : IInputMode
    {
        public string Text => "Ka";

        public bool AcceptsKey(int scanCode)
        {
            return scanCode >= 48 && scanCode <= 57 || scanCode >= 97 && scanCode <= 122;
        }

        public string GetPreview(string data)
        {
            return KatakanaMapping.Mapping.TryGetValue(data, out char character) ? character.ToString() : "";
        }

        public Tuple<char, char?> GetResult(string data)
        {
            return KatakanaMapping.Mapping.TryGetValue(data, out char character) ? new Tuple<char, char?>(character, null) : null;
        }
    }
}
