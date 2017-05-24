using System;

namespace domi1819.UniType
{
    internal interface IInputMode
    {
        string Text { get; }

        bool AcceptsKey(int scanCode);

        string GetPreview(string data);

        Tuple<char, char?> GetResult(string data);
    }
}
