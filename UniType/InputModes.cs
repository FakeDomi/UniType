namespace domi1819.UniType
{
    internal class InputModes
    {
        private readonly IInputMode[] modes = { new InputModeUnicode(), new InputModeKatakana() };

        private int currentIndex;

        internal IInputMode Current => this.modes[this.currentIndex];

        internal void Next()
        {
            if (this.currentIndex == this.modes.Length - 1)
            {
                this.currentIndex = 0;
            }
            else
            {
                this.currentIndex++;
            }
        }
    }
}
